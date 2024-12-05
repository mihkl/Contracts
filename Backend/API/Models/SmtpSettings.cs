using System.ComponentModel.DataAnnotations;

public record SmtpSettings
{
    [Key]
    public int Id { get; set; }
    public required string UserId { get; set; }
    public required string Host { get; set; }
    public required int Port { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; }
    public string? FromEmail { get; set; }
    public string? NotifyOnUploadContent { get; set; }
    public string? NotifyOnSignatureContent { get; set; }
    public string? SignatureNotificationEmail { get; set; }
}