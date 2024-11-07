using System.ComponentModel.DataAnnotations;
using API.Models;

public record ContractSignature
{
    [Key]
    public uint Id { get; init; }
    public byte[] FileData { get; set; } = [];
    public uint ContractId { get; set; }
    public DateTime UploadedAt { get; set; }
    public ContractSignatureType Type { get; set; }

    public Contract? Contract { get; set; }
}

public enum ContractSignatureType
{
    Candidate = 0,
    CompanyRepresentative = 1
}