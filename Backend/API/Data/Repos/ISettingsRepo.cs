using API.Models.Requests;

namespace API.Data.Repos
{
    public interface ISettingsRepo
    {
        Task<SmtpSettings?> GetSmtpSettings(string userId);
        Task AddUpdateSmtpSettings(string userId, AddUpdateSmtpSettings smtpSettings);

    }
}