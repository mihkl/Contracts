using API.Models;

public record UpdateContract
{
    public string? Url { get; init; } = null;
    public SigningStatus? SigningStatus { get; init; } = null;
}