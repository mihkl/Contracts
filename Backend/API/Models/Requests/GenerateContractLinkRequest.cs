namespace API.Models;
public record GenerateContractLinkRequest
{
    public string Name { get; init; } = string.Empty;
}