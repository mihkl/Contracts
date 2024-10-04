namespace API.Models;

public record NewFromTemplateRequest
{
    public uint TemplateId { get; init; }
    public string Name { get; init; } = string.Empty;
}