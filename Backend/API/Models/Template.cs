using System.ComponentModel.DataAnnotations;

namespace API.Models;

public record Template
{
    [Key]
    public uint Id { get; init; }

    public string Name { get; set; } = string.Empty;

    public byte[] FileData { get; init; } = [];

    public List<TemplateDynamicField> Fields { get; init; } = [];
}

public record TemplateDto
{
    public uint Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public List<TemplateDynamicFieldDto> Fields { get; init; } = [];
}
