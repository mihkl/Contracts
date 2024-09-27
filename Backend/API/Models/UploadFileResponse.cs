namespace API.Models;

public record UploadFileResponse
{
    public required ContractDto Contract { get; init; }
    public required Guid Guid { get; init; }
}
