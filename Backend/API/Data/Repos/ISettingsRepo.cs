using API.Models.Requests;

namespace API.Data.Repos
{
    public interface ISettingsRepo
    {
        Task<SmtpSettings?> GetSmtpSettings(string userId);
        Task AddUpdateSmtpSettings(string userId, AddUpdateSmtpSettings smtpSettings);
        Task DeleteSmtpSettings(string userId);
        Task<string?> GetSendFinalContractEmailContent(string userId);
        Task<string?> GetSendSignedContractUploadEmailContent(string userId);
        Task<string?> GetSignedContractUploadNotificationEmailAddress(string userId);
    }
}