using API.Data.Repos;
using MailKit.Net.Smtp;

namespace API.emails
{
    public interface IEmailsService
    {
        Task SendEmailsAsync(List<EmailMessage> emailMessages, string userId);
    }
    public class EmailsService(ISettingsRepo settingsRepo) : IEmailsService
    {
        private readonly ISettingsRepo _settingsRepo = settingsRepo;
        public async Task SendEmailsAsync(List<EmailMessage> emailMessages, string userId)
        {
            var smtpClient = await GetSmtpClient(userId);
        }

        private async Task<SmtpClient> GetSmtpClient(string userId)
        {
            var smtpClient = new SmtpClient();

            var smtpSettings = await _settingsRepo.GetSmtpSettings(userId);
            return smtpClient;
        }

        private async Task DisconnectSmtpClient(SmtpClient client)
        {
            await client.DisconnectAsync(true);
        }
    }
}