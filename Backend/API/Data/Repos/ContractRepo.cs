using API.FileManipulation;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repos;

public class ContractRepo(DataContext context) : IContractRepo
{
    private readonly DataContext _context = context;
    public async Task<Contract> SaveContractToDb(Contract contract)
    {
        _context.Add(contract);
        await SaveChangesAsync();
        return contract;
    }

    public async Task<List<Contract>> GetContractsFromDb()
    {
        return await _context.Contracts
        .Include(c => c.Fields)
        .ToListAsync();
    }

    public async Task<Contract?> GetContractByIdFromDb(uint id)
    {
        return await _context.Contracts
        .Include(c => c.Fields)
        .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task DeleteContractFromDb(Contract contract)
    {
        _context.Remove(contract);
        await SaveChangesAsync();
    }

    public async Task ReplaceDynamicFields(List<DynamicFieldReplacement> replacements, Contract contract)
    {
        contract.FileData = FileManipulator.ReplacePlaceholdersInDocument(contract, replacements);
        UpdateDynamicFields(contract, replacements);

        await SaveChangesAsync();
    }

    private static void UpdateDynamicFields(Contract contract, List<DynamicFieldReplacement> replacements)
    {
        replacements.Where(r => contract.Fields.Any(f => f.Name == r.Name))
                            .ToList()
                            .ForEach(r => contract.Fields.First(f => f.Name == r.Name).Value = r.Value);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
