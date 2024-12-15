using API.Data.Repos;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

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
            try
            {
                var smtpSettings = await _settingsRepo.GetSmtpSettings(userId);

                if (smtpSettings == null) return;

                await SendEmailsInternalAsync(emailMessages, smtpSettings);

            }
            catch (Exception ex)
            {
                Console.WriteLine("There has been an exception! " + ex.ToString());
            }
        }

        private async Task SendEmailsInternalAsync(List<EmailMessage> emailMessages, SmtpSettings smtpSettings)
        {
            var smtpClient = await GetSmtpClient(smtpSettings!);

            foreach (var email in emailMessages)
            {
                MimeMessage emailMessage = new MimeMessage();
                emailMessage.From.Add(MailboxAddress.Parse(smtpSettings!.FromEmail));
                emailMessage.To.Add(MailboxAddress.Parse(email.To));
                emailMessage.Subject = email.Subject;

                BodyBuilder builder = new BodyBuilder();
                builder.HtmlBody = email.Content;

                if (email.Attachment != null) builder.Attachments.Add("contract.asice", email.Attachment);

                emailMessage.Body = builder.ToMessageBody();

                await smtpClient.SendAsync(emailMessage);

            }

            await DisconnectSmtpClient(smtpClient);
        }
        private async Task<SmtpClient> GetSmtpClient(SmtpSettings smtpSettings)
        {
            var smtpClient = new SmtpClient();

            await smtpClient.ConnectAsync(smtpSettings!.Host, smtpSettings.Port, SecureSocketOptions.StartTls);

            if (!string.IsNullOrWhiteSpace(smtpSettings.Username) && !string.IsNullOrWhiteSpace(smtpSettings.Password))
            {
                await smtpClient.AuthenticateAsync(smtpSettings.Username, smtpSettings.Password);
            }

            return smtpClient;
        }

        private async Task DisconnectSmtpClient(SmtpClient client)
        {
            await client.DisconnectAsync(true);
        }
    }
}