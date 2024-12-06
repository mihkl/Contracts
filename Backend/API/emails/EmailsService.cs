namespace API.emails
{
    public interface IEmailsService
    {
        Task SendEmailsAsync(List<EmailMessage> emailMessages);
    }
    public class EmailsService : IEmailsService
    {
        public Task SendEmailsAsync(List<EmailMessage> emailMessages)
        {
            throw new NotImplementedException();
        }
    }
}