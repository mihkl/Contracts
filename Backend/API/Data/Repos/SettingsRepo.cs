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
                NotifyOnUploadSubject = smtpSettings.NotifyOnUploadSubject,
                NotifyOnUploadContent = smtpSettings.NotifyOnUploadContent,
                NotifyOnSignatureSubject = smtpSettings.NotifyOnSignatureSubject,
                NotifyOnSignatureContent = smtpSettings.NotifyOnSignatureContent,
                SignatureNotificationEmail = smtpSettings.SignatureNotificationEmail,
                DocumentIsAttached = smtpSettings.DocumentIsAttached,
                NotificationDocumentIsAttached = smtpSettings.NotificationDocumentIsAttached
            });

            else
            {
                currentSettings.Port = smtpSettings.Port;
                currentSettings.Password = smtpSettings.Password;
                currentSettings.Host = smtpSettings.Host;
                currentSettings.Username = smtpSettings.Username;
                currentSettings.FromEmail = smtpSettings.FromEmail;
                currentSettings.NotifyOnUploadSubject = smtpSettings.NotifyOnUploadSubject;
                currentSettings.NotifyOnUploadContent = smtpSettings.NotifyOnUploadContent;
                currentSettings.NotifyOnSignatureSubject = smtpSettings.NotifyOnSignatureSubject;
                currentSettings.NotifyOnSignatureContent = smtpSettings.NotifyOnSignatureContent;
                currentSettings.SignatureNotificationEmail = smtpSettings.SignatureNotificationEmail;
                currentSettings.DocumentIsAttached = smtpSettings.DocumentIsAttached;
                currentSettings.NotificationDocumentIsAttached = smtpSettings.NotificationDocumentIsAttached;
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

        public async Task<string?> GetSignedContractUploadNotificationEmailAddress(string userId)
        {
            var settings = await _context.SmtpSettings
                .Where(x => x.UserId == userId)
                .FirstOrDefaultAsync();

            if (settings == null) return null;

            return settings.SignatureNotificationEmail;

        }

        public async Task<SmtpSettings?> GetSmtpSettings(string userId)
        {
            var settings = await _context.SmtpSettings
                .Where(x => x.UserId == userId)
                .FirstOrDefaultAsync();

            return settings;
        }

        public async Task<(string content, string subject)?> GetSendSignedContractUploadEmailContentAndSubject(string userId)
        {
            var settings = await _context.SmtpSettings
                .Where(x => x.UserId == userId)
                .FirstOrDefaultAsync();

            if (settings == null) return null;

            return (settings.NotifyOnSignatureContent, settings.NotifyOnSignatureSubject);
        }

        public async Task<string?> GetSendSignedContractUploadEmailContent(string userId)
        {
            var settings = await _context.SmtpSettings
                .Where(x => x.UserId == userId)
                .FirstOrDefaultAsync();

            if (settings == null) return null;

            return settings.NotifyOnSignatureContent;
        }

        public async Task<(string content, string subject)?> GetSendFinalContractEmailContentAndSubject(string userId)
        {
            var settings = await _context.SmtpSettings
                .Where(x => x.UserId == userId)
                .FirstOrDefaultAsync();

            if (settings == null) return null;

            return (settings.NotifyOnUploadContent, settings.NotifyOnUploadSubject);
        }

        public async Task<bool?> IncludeAttachmentInContractUploadNotification(string userId)
        {
            var settings = await _context.SmtpSettings
                .Where(x => x.UserId == userId)
                .FirstOrDefaultAsync();

            if (settings == null) return false;

            return settings.NotificationDocumentIsAttached;
        }

        public async Task<bool?> IncludeAttachmentInFinalContractNotification(string userId)
        {
            var settings = await _context.SmtpSettings
                .Where(x => x.UserId == userId)
                .FirstOrDefaultAsync();

            if (settings == null) return false;

            return settings.DocumentIsAttached;
        }
    }
}