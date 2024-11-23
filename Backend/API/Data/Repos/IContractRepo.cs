using API.Models;

namespace API.Data.Repos;

public interface IContractRepo : IRepo<Contract>
{
    Task<List<Contract>> GetAll(string? userId, SigningStatus? minimumStatus);
    Task<string> ReplaceDynamicFields(List<ContractDynamicFieldReplacement> replacements, Contract contract, Template template);
}
