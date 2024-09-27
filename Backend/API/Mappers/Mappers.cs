using API.Models;
namespace API.Mappers;

public static class Mappers
{
    public static ContractDto ToContractDto(Contract contract) => new()
    {
        Id = contract.Id,
        Name = contract.Name,
        Fields = contract.Fields.Select(ToDynamicFieldDto).ToList()
    };

    public static DynamicFieldDto ToDynamicFieldDto(DynamicField field) => new()
    {
        Name = field.Name,
        Value = field.Value,
        Type = field.Type
    };
}
