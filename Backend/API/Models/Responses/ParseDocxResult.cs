namespace API.Models.Responses;

public record ParseDocxResult
{
    public Template? Template { get; init; }
    public string? InfoMessage { get; init; }
}
