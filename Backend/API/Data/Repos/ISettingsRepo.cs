using API.Models.Requests;

namespace API.Data.Repos
{
    public interface ISettingsRepo
    {
        Task<SmtpSettings?> GetSmtpSettings(string userId);
        Task AddUpdateSmtpSettings(string userId, AddUpdateSmtpSettings smtpSettings);
        Task DeleteSmtpSettings(string userId);
        Task<(string content, string subject)?> GetSendFinalContractEmailContentAndSubject(string userId);
        Task<(string content, string subject)?> GetSendSignedContractUploadEmailContentAndSubject(string userId);
        Task<string?> GetSignedContractUploadNotificationEmailAddress(string userId);
        Task<bool?> IncludeAttachmentInFinalContractNotification(string userId);
        Task<bool?> IncludeAttachmentInContractUploadNotification(string userId);
    }
}