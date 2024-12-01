namespace API.Models.Responses;

public record UploadFileResponse
{
    public required TemplateDto Template { get; init; }
    public required Guid Guid { get; init; }
    public string? InfoMessage { get; init; }
}
