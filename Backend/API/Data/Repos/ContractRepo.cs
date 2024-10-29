using API.FileManipulation;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repos;

public class ContractRepo(DataContext context) : IContractRepo
{
    private readonly DataContext _context = context;
    public async Task<Contract> Save(Contract contract)
    {
        _context.Add(contract);
        await SaveChangesAsync();
        return contract;
    }

    public async Task<List<Contract>> GetAll()
    {
        return await _context.Contracts
        .Include(c => c.Fields)
        .ToListAsync();
    }

    public async Task<Contract?> GetById(uint id)
    {
        return await _context.Contracts
        .Include(c => c.Fields)
        .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task Delete(Contract contract)
    {
        _context.Remove(contract);
        await SaveChangesAsync();
    }

    public async Task ReplaceDynamicFields(List<ContractDynamicFieldReplacement> replacements, Contract contract)
    {
        contract.FileData = FileManipulator.ReplaceContractPlaceholders(contract, replacements);
        UpdateDynamicFields(contract, replacements);

        await SaveChangesAsync();
    }

    private static void UpdateDynamicFields(Contract contract, List<ContractDynamicFieldReplacement> replacements)
    {
        replacements.Where(r => contract.Fields.Any(f => f.Name == r.Name))
                            .ToList()
                            .ForEach(r => contract.Fields.First(f => f.Name == r.Name).Value = r.Value);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task Update(uint id, UpdateContract updateContract)
    {
        var contract = await GetById(id);
        var properties = typeof(UpdateContract).GetProperties();
        var contractProperties = typeof(Contract).GetProperties();

        foreach (var property in properties)
        {
            var dtoValue = property.GetValue(updateContract);
            if (dtoValue != null)
            {
                var entityProp = Array.Find(contractProperties, p => p.Name == property.Name);
                if (entityProp != null && entityProp.CanWrite)
                {
                    entityProp.SetValue(contract, dtoValue);
                }
            }
        }

        await SaveChangesAsync();
    }
}
