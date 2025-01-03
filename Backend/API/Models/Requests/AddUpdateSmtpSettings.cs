namespace API.Models.Requests
{
    public class AddUpdateSmtpSettings
    {
        public required string Host { get; set; }
        public required int Port { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? FromEmail { get; set; }
        public string? NotifyOnUploadSubject { get; set; }
        public string? NotifyOnUploadContent { get; set; }
            public string? NotifyOnSignatureSubject { get; set; }
        public string? NotifyOnSignatureContent { get; set; }
        public string? SignatureNotificationEmail { get; set; }
        public bool? DocumentIsAttached { get; set; }
        public bool? NotificationDocumentIsAttached { get; set; }
    }
}