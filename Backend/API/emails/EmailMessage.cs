namespace API.emails
{
    public class EmailMessage(int id, string to, DateTime sentAt, EmailMessage.EmailMessageType type)
    {
        public int Id { get; init; } = id;
        public string To { get; init; } = to;
        public DateTime SentAt { get; init; } = sentAt;
        public EmailMessageType Type { get; init; } = type;

        public enum EmailMessageType
        {
            FinalContractNotification = 0,
            SignedContractReceivedNotification = 1,
        }
    }



}