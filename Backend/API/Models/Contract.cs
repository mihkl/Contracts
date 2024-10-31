using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models;

public record Contract
{
    [Key]
    public uint Id { get; init; }
    public string Name { get; set; } = string.Empty;
    public byte[] FileData { get; set; } = [];
    public List<ContractDynamicField> Fields { get; set; } = [];
    public string Url { get; set; } = string.Empty;
    public ContractStatus ContractStatus { get; set; }
}

public record ContractDto
{
    public uint Id { get; init; }
    public string Name { get; set; } = string.Empty;
    public List<ContractDynamicFieldDto> Fields { get; set; } = [];
    public string Url { get; set; } = string.Empty;
}

public enum ContractStatus
{
    LinkGenerated,
    ContractSigned,
    ContractCounterSigned,
    FinalContractSentToApplicant
}


