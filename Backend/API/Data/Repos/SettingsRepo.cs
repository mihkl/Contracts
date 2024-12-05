using API.Models.Requests;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repos
{
    public class SettingsRepo(DataContext context) : ISettingsRepo
    {
        private readonly DataContext _context = context;

        public async Task AddUpdateSmtpSettings(string userId, AddUpdateSmtpSettings smtpSettings)
        {
            var currentSettings = await GetSmtpSettings(userId);

            if (currentSettings == null) _context.SmtpSettings.Add(new SmtpSettings
            {
                UserId = userId,
                Host = smtpSettings.Host,
                Port = smtpSettings.Port,
                Username = smtpSettings.Username,
                Password = smtpSettings.Password,
                FromEmail = smtpSettings.FromEmail,
                NotifyOnUploadContent = smtpSettings.NotifyOnUploadContent,
                NotifyOnSignatureContent = smtpSettings.NotifyOnSignatureContent,
                SignatureNotificationEmail = smtpSettings.SignatureNotificationEmail // New
            });

            else
            {
                currentSettings.Port = smtpSettings.Port;
                currentSettings.Password = smtpSettings.Password;
                currentSettings.Host = smtpSettings.Host;
                currentSettings.Username = smtpSettings.Username;
                currentSettings.FromEmail = smtpSettings.FromEmail;
                currentSettings.NotifyOnUploadContent = smtpSettings.NotifyOnUploadContent;
                currentSettings.NotifyOnSignatureContent = smtpSettings.NotifyOnSignatureContent;
                currentSettings.SignatureNotificationEmail = smtpSettings.SignatureNotificationEmail;
            }

            await _context.SaveChangesAsync();
        }

        public async Task DeleteSmtpSettings(string userId)
        {
            var settings = await _context.SmtpSettings
                .Where(x => x.UserId == userId)
                .FirstOrDefaultAsync();

            if (settings == null) return;

            _context.Remove(settings);
            await _context.SaveChangesAsync();
        }

        public async Task<SmtpSettings?> GetSmtpSettings(string userId)
        {
            var settings = await _context.SmtpSettings
                .Where(x => x.UserId == userId)
                .FirstOrDefaultAsync();

            return settings;
        }
    }
}