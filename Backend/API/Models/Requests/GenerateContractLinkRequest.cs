namespace API.Models;
public record GenerateContractLinkRequest
{
    public string Name { get; init; } = string.Empty;
    public DateTime ValidFrom { get; init; }
    public DateTime ValidUntil { get; init; }
}