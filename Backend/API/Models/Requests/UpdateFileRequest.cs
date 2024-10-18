namespace API.Models;

public record UpdateFileRequest
{
    public List<ContractDynamicFieldReplacement> Replacements { get; set; } = [];
}
