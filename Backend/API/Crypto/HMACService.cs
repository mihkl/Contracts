using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace API.Controllers
{
    public interface IHMACService
    {
        string GenerateSignature(DateTime startDate, DateTime endDate, string templateId);
        bool IsValidSignature(DateTime startDate, DateTime endDate, string templateId, string signature);
        string FormatDate(DateTime date);
    }

    public class HMACService : IHMACService
    {
        private byte[] _hmacSecretKey = Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("HMACSecretKey") ?? "");
        public string GenerateSignature(DateTime startDate, DateTime endDate, string templateId)
        {
            var message = generateMessage(startDate, endDate, templateId);
            return GenerateHmac(message);
        }

        public bool IsValidSignature(DateTime startDate, DateTime endDate, string templateId, string signature)
        {
            var message = generateMessage(startDate, endDate, templateId);
            var expectedSignature = GenerateHmac(message);

            Console.WriteLine("EXpected is " + expectedSignature);
            Console.WriteLine("Real is " + signature);

            return expectedSignature == signature;
        }

        public string FormatDate(DateTime date)
        {
            return date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        }

        private string generateMessage(DateTime startDate, DateTime endDate, string templateId)
        {
            Console.WriteLine("validfrom is " + FormatDate(startDate));
            Console.WriteLine("validuntil is " + FormatDate(endDate));
            Console.WriteLine("id is " + templateId);

            return $"{FormatDate(startDate)}|{FormatDate(endDate)}|{templateId}";
        }

        private string GenerateHmac(string message)
        {
            var encoding = Encoding.UTF8;
            var messageBytes = encoding.GetBytes(message);

            using (var hmac = new HMACSHA256(_hmacSecretKey))
            {
                var hashBytes = hmac.ComputeHash(messageBytes);
                return Convert.ToBase64String(hashBytes);
            }
        }

    }

}