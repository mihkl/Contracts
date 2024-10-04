namespace API.Models;

public record UploadFileResponse
{
    public required TemplateDto Template { get; init; }
    public required Guid Guid { get; init; }
}
