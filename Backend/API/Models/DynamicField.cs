using System.ComponentModel.DataAnnotations;

namespace API.Models;

public record ContractDynamicField
{
    [Key]
    public uint Id { get; init; }
    public string Placeholder { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public string Type { get; init; } = string.Empty;
}

public record TemplateDynamicField 
{
    [Key]
    public uint Id { get; init; }
    public string Placeholder { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string Type { get; init; } = string.Empty;
}

public record TemplateDynamicFieldDto
{
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
}

public record ContractDynamicFieldDto
{
    public string Name { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
}

public record ContractDynamicFieldReplacement
{
    public string Name { get; init; } = string.Empty;
    public string Value { get; init; } = string.Empty;
}