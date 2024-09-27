using API.Models;

namespace API.Data.Repos;

public interface IContractRepo
{
    Task<Contract> SaveContractToDb(Contract contract);
    Task<List<Contract>> GetContractsFromDb();
    Task<Contract?> GetContractByIdFromDb(uint id);
    Task DeleteContractFromDb(Contract contract);
    Task ReplaceDynamicFields(List<DynamicFieldReplacement> replacements, Contract contract);
    Task SaveChangesAsync();
}
