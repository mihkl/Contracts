using API.Models;

namespace API.Data.Repos;

public interface IContractRepo : IRepo<Contract>
{
    Task ReplaceDynamicFields(List<ContractDynamicFieldReplacement> replacements, Contract contract);
}
