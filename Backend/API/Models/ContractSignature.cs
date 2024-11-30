using System.ComponentModel.DataAnnotations;
using API.Models;

public record ContractSignature
{
    [Key]
    public uint Id { get; init; }
    public string FilePath { get; init; }
    public uint ContractId { get; set; }
    public DateTime UploadedAt { get; set; }
    public ContractSignatureType Type { get; set; }
}

public enum ContractSignatureType
{
    Candidate = 0,
    CompanyRepresentative = 1
}