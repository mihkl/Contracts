using Microsoft.EntityFrameworkCore;

namespace API.Data.Repos
{
    public class SettingsRepo(DataContext context) : ISettingsRepo
    {
        private readonly DataContext _context = context;

        public async Task<SmtpSettings> GetSmtpSettings(string userId)
        {
            var settings = await _context.SmtpSettings
                .Where(x => x.UserId == userId)
                .FirstAsync();

            return settings;
        }
    }
}