namespace API.Models;

public record UpdateFileRequest
{
    public uint ContractId { get; set; }
    public List<ContractDynamicFieldReplacement> Replacements { get; set; } = [];
}
