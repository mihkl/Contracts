using API.FileManipulation;
using API.Models;
using API.Validation;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repos;

public class ContractRepo(DataContext context) : IContractRepo
{
    private readonly DataContext _context = context;
    public async Task<Contract> Save(Contract contract, string? userId)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null)
        {
            return null;
        }
        contract.CreationTime = DateTime.UtcNow;
        _context.Add(contract);
        user.Contracts.Add(contract);
        await SaveChangesAsync();
        return contract;
    }

    public async Task<List<Contract>> GetAll(string? userId, SigningStatus? status)
    {
        var user = await _context.Users
            .Include(u => u.Contracts)
                .ThenInclude(c => c.Fields)
            .Include(u => u.Contracts)
                .ThenInclude(c => c.SubmittedFields)
            .Include(c => c.Contracts)
                    .ThenInclude(c => c.Signatures)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null) return [];

        if (status != null)
        {
            user.Contracts = user.Contracts
                .Where(c => c.SigningStatus == status)
                .ToList();
        }

        return user!.Contracts.ToList();
    }

    public async Task<Contract?> GetById(uint id, string? userId)
    {
        var user = await _context.Users
            .Include(u => u.Contracts)
                .ThenInclude(c => c.Fields)
            .Include(u => u.Contracts)
                .ThenInclude(c => c.SubmittedFields)
            .FirstOrDefaultAsync(u => u.Id == userId);

        return user?.Contracts.FirstOrDefault(c => c.Id == id);
    }

    public async Task<Contract?> GetById(uint id)
    {
        return await _context.Contracts
        .Include(c => c.Fields)
        .Include(c => c.SubmittedFields)
        .Include(c => c.Signatures)
        .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task Delete(Contract contract)
    {
        _context.Remove(contract);
        await SaveChangesAsync();
    }

    public async Task<string> ReplaceDynamicFields(List<ContractDynamicFieldReplacement> replacements, Contract contract, Template template)
    {
        if (!ValidateReplacements(replacements, template, out var error))
        {
            return error;
        }
        contract.FileData = FileManipulator.ReplaceContractPlaceholders(replacements, template);
        UpdateDynamicFields(contract, replacements);

        await SaveChangesAsync();
        return string.Empty;
    }

    private static bool ValidateReplacements(List<ContractDynamicFieldReplacement> replacements, Template template, out string error)
    {
        var replacementsWithType = replacements.Select(r => new ContractDynamicFieldDto
        {
            Name = r.Name,
            Value = r.Value,
            Type = template.Fields.First(f => f.Name == r.Name).Type
        }).ToArray();

        return FormValidator.Validate(replacementsWithType, out error);
    }


    private void UpdateDynamicFields(Contract contract, List<ContractDynamicFieldReplacement> replacements)
    {
        replacements.Where(r => contract.Fields.Any(f => f.Name == r.Name))
                            .ToList()
                            .ForEach(r => contract.Fields.First(f => f.Name == r.Name).Value = r.Value);

        _context.RemoveRange(contract.SubmittedFields);
        contract.SubmittedFields.Clear();
        contract.SubmittedFields.AddRange(replacements);
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

    public async Task<ContractSignature> SaveSignature(Contract contract, byte[] parsedFile, ContractSignatureType signatureType)
    {
        string baseDirectory = AppContext.BaseDirectory;

        string signedContractsFolder = Path.Combine(baseDirectory, "SignedContracts");

        string signedContractPath = Path.Combine(signedContractsFolder, contract.Name + new Random().Next() + ".asice");

        await File.WriteAllBytesAsync(signedContractPath, parsedFile!);

        var contractSignature = new ContractSignature
        {
            FilePath = signedContractPath,
            ContractId = contract.Id,
            Type = signatureType,
        };

        using var transaction = await _context.Database.BeginTransactionAsync();

        await _context.AddAsync(contractSignature);
        await SaveChangesAsync();

        await Update(contract.Id, new UpdateContract
        {
            SigningStatus = signatureType == ContractSignatureType.Candidate ?
                SigningStatus.SignedByFirstParty : SigningStatus.SignedByAll
        });

        await transaction.CommitAsync();

        return contractSignature;
    }
}
