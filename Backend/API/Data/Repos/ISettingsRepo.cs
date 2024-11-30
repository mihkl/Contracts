namespace API.Data.Repos
{
    public interface ISettingsRepo
    {
        Task<SmtpSettings> GetSmtpSettings(string userId);
    }
}