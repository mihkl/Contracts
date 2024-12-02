namespace API.Models.Requests
{
    public class AddUpdateSmtpSettings
    {
        public required string Host { get; set; }
        public required int Port { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? FromEmail { get; set; }
    }
}