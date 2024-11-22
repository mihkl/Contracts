using System.ComponentModel.DataAnnotations;
using System.Globalization;
using API.FileManipulation;
using API.Models;
using API.Models.Requests;
using API.Validation;
using IbanNet;
using Microsoft.EntityFrameworkCore;
using OpenXmlPowerTools;

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
        _context.Add(contract);
        user.Contracts.Add(contract);
        await SaveChangesAsync();
        return contract;
    }

    public async Task<List<Contract>> GetAll(string? userId, GetContractsDto? dto)
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

        if (dto?.MinimumStage != null)
        {
            user.Contracts = user.Contracts
                .Where(c => c.SigningStatus >= dto.MinimumStage)
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

    public async Task<ContractSignature> SaveSignatureAndUpdateContractStatus(ContractSignature signature)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        await _context.AddAsync(signature);
        await SaveChangesAsync();

        // siin loogika muutub. Signingstatuse uuendamiseks peab enne ilmselt chekima, mis type´i üles laetav signatuur on ja mis on praegune contracti staatus.
        await Update(signature.Id, new UpdateContract
        {
            SigningStatus = SigningStatus.SignedByFirstParty
        });

        await transaction.CommitAsync();

        return signature;
    }
}
