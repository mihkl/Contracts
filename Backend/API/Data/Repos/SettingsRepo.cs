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
                FromEmail = smtpSettings.FromEmail
            });

            else
            {
                currentSettings.Port = smtpSettings.Port;
                currentSettings.Password = smtpSettings.Password;
                currentSettings.Host = smtpSettings.Host;
                currentSettings.Username = smtpSettings.Username;
                currentSettings.FromEmail = smtpSettings.FromEmail;
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

        public async Task<bool> IsEmailIntegrationEnabled(string userId)
        {
            var settings = await _context.SmtpSettings
                .Where(x => x.UserId == userId)
                .FirstOrDefaultAsync();

            if (settings == null) return false;

            return !string.IsNullOrWhiteSpace(settings.Host) && !string.IsNullOrWhiteSpace(settings.FromEmail);
        }
    }
}