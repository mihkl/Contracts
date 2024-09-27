using System.ComponentModel.DataAnnotations;

namespace API.Models;

public record DynamicField
{
    [Key]
    public uint Id { get; init; }
    public string Placeholder { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public string Type { get; init; } = string.Empty;
}

public record DynamicFieldDto
{
    public string Name { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
}

public record DynamicFieldReplacement
{
    public string Name { get; init; } = string.Empty;
    public string Value { get; init; } = string.Empty;
}