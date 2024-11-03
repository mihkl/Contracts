using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace API.Models;

public record Contract
{
    [Key]
    public uint Id { get; init; }
    public string Name { get; set; } = string.Empty;
    public byte[] FileData { get; set; } = [];
    public List<ContractDynamicField> Fields { get; set; } = [];
    public string Url { get; set; } = string.Empty;
    public SigningStatus SigningStatus { get; set; } = SigningStatus.SignedByNone;
    public List<ContractDynamicFieldReplacement> SubmittedFields { get; set; } = [];
    public DateTime? LinkValidFrom { get; set; }
    public DateTime? LinkValidUntil { get; set; }
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum SigningStatus
{
    SignedByNone = 0,
    SignedByFirstParty = 1,
    SignedBySecondParty = 2,
    SignedByAll = 3,
}


public record ContractDto
{
    public uint Id { get; init; }
    public string Name { get; set; } = string.Empty;
    public List<ContractDynamicFieldDto> Fields { get; set; } = [];
    public string Url { get; set; } = string.Empty;
    public SigningStatus SigningStatus { get; set; } = SigningStatus.SignedByNone;
    public List<ContractDynamicFieldReplacement> SubmittedFields { get; set; } = [];
    public DateTime? LinkValidFrom { get; set; }
    public DateTime? LinkValidUntil { get; set; }

}


