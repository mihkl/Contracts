using API.Models;
namespace API.Mappers;

public static class Mappers
{
    public static ContractDto ToContractDto(Contract contract) => new()
    {
        Id = contract.Id,
        Name = contract.Name,
        Fields = contract.Fields.Select(ToContractDynamicFieldDto).ToList()
    };

    public static ContractDynamicFieldDto ToContractDynamicFieldDto(ContractDynamicField field) => new()
    {
        Name = field.Name,
        Value = field.Value,
        Type = field.Type
    };

    public static TemplateDto ToTemplateDto(Template template) => new()
    {
        Id = template.Id,
        Name = template.Name,
        Fields = template.Fields.Select(ToTemplateDynamicFieldDto).ToList()
    };

    public static TemplateDynamicFieldDto ToTemplateDynamicFieldDto(TemplateDynamicField field) => new()
    {
        Name = field.Name,
        Type = field.Type
    };

    public static ContractDynamicField ToContractDynamicField(TemplateDynamicField field) => new()
    {
        Id = field.Id,
        Placeholder = field.Placeholder,
        Name = field.Name,
        Value = string.Empty,
        Type = field.Type
    };
}
