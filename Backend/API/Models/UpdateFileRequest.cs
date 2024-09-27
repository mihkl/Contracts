namespace API.Models;

public record UpdateFileRequest
{
    public uint ContractId { get; set; }
    public List<DynamicFieldReplacement> Replacements { get; set; } = [];
}
