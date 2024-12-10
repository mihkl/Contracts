namespace API.emails
{
    public class EmailMessage(string to, DateTime sentAt, EmailMessage.EmailMessageType type, string content, string subject)
    {
        public string To { get; init; } = to;
        public DateTime SentAt { get; init; } = sentAt;
        public EmailMessageType Type { get; init; } = type;
        public string Content { get; init; } = content;
        public string Subject { get; init; } = subject;

        public enum EmailMessageType
        {
            FinalContractNotification = 0,
            SignedContractReceivedNotification = 1,
        }
    }



}