namespace API.Models;
public record SaveTemplateRequest
{
    public Guid Guid { get; init; }
    public string Name { get; init; } = string.Empty;
}
