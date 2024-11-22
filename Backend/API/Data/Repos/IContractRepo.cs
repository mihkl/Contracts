using API.Models;
using API.Models.Requests;

namespace API.Data.Repos;

public interface IContractRepo : IRepo<Contract>
{
    Task<List<Contract>> GetAll(string? userId, GetContractsDto? dto);
    Task<string> ReplaceDynamicFields(List<ContractDynamicFieldReplacement> replacements, Contract contract, Template template);
}
