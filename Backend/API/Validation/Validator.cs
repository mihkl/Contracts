using System.IO.Compression;
using API.Models;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using DocumentFormat.OpenXml.Packaging;
using IbanNet;
using UglyToad.PdfPig;

namespace API.Validation;

public static class FormValidator
{
    public static bool Validate(ContractDynamicFieldDto[] replacementFields, out string error)
    {
        error = string.Empty;

        var validators = new Dictionary<string, Func<string, bool>>()
        {
            { "string", value => !string.IsNullOrWhiteSpace(value) },
            { "number", IsValidPhoneNumber },
            { "email", IsValidEmail },
            { "date", value => IsValidDateOfBirth(value, out _) },
            { "address", IsValidAddress },
            { "iban", IsValidBankAccountNumber },
            { "ssn", IsValidIsikukood }
        };

        foreach (var field in replacementFields)
        {
            if (validators.TryGetValue(field.Type, out var validator))
            {
                if (!validator(field.Value))
                {
                    error = $"Invalid value for field {field.Name}.";
                    return false;
                }
            }
        }

        var dateOfBirthField = replacementFields.FirstOrDefault(f => f.Name == "Date of birth");
        var ssnField = replacementFields.FirstOrDefault(f => f.Name == "Social security number");

        if (dateOfBirthField is not null && ssnField is not null && !IsIsikukoodAndDateOfBirthMatch(ssnField.Value, dateOfBirthField.Value))
        {
            error = "Social security number and date of birth do not match.";
            return false;
        }
        return true;
    }
    public static bool IsValidEmail(string email) => new System.ComponentModel.DataAnnotations.EmailAddressAttribute().IsValid(email);

    public static bool IsValidPhoneNumber(string phoneNumber) => !phoneNumber.Contains('-') && ulong.TryParse(phoneNumber, out ulong _);

    public static bool IsValidDateOfBirth(string dateOfBirth, out DateTime parsedDate) => DateTime.TryParse(dateOfBirth, out parsedDate);

    public static bool IsValidAddress(string address) => !string.IsNullOrWhiteSpace(address);

    public static bool IsValidBankAccountNumber(string bankAccountNumber) => new IbanValidator().Validate(bankAccountNumber).IsValid;

    public static bool IsIsikukoodAndDateOfBirthMatch(string isikukood, string dateOfBirth)
    {
        string isikukoodDateOfBirth = GetBirthDateFromIsikukood(isikukood);
        var isikukoodDateOfBirthParsed = DateTime.ParseExact(isikukoodDateOfBirth, "yyyyMMdd", null);
        var dateOfBirthParsed = DateTime.Parse(dateOfBirth);
        return isikukoodDateOfBirthParsed == dateOfBirthParsed;
    }

    public static bool IsValidIsikukood(string isikukood)
    {
        if (isikukood.Length != 11 || !long.TryParse(isikukood, out _)) return false;

        int genderCentury = int.Parse(isikukood[0].ToString());
        if (genderCentury < 1 || genderCentury > 8) return false;

        string birthDateString = GetBirthDateFromIsikukood(isikukood);
        if (!DateTime.TryParseExact(birthDateString, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out _))
            return false;

        int[] weights1 = [1, 2, 3, 4, 5, 6, 7, 8, 9, 1];
        int[] weights2 = [3, 4, 5, 6, 7, 8, 9, 1, 2, 3];

        int checksum = CalculateChecksum(isikukood, weights1);
        if (checksum == 10) checksum = CalculateChecksum(isikukood, weights2);
        if (checksum == 10) checksum = 0;

        return checksum == int.Parse(isikukood[10].ToString());
    }

    private static string GetBirthDateFromIsikukood(string isikukood)
    {
        int century = int.Parse(isikukood[0].ToString());
        string yearPrefix = century switch
        {
            1 or 2 => "18",
            3 or 4 => "19",
            5 or 6 => "20",
            7 or 8 => "21",
            _ => throw new ArgumentException("Invalid century in isikukood.")
        };

        string year = yearPrefix + isikukood.Substring(1, 2);
        string month = isikukood.Substring(3, 2);
        string day = isikukood.Substring(5, 2);

        return $"{year}{month}{day}";
    }

    private static int CalculateChecksum(string isikukood, int[] weights)
    {
        int sum = 0;
        for (int i = 0; i < 10; i++)
        {
            sum += (isikukood[i] - '0') * weights[i];
        }
        return sum % 11;
    }
}

public class AsiceValidator
{
    private string _tempAsicePath = string.Empty;
    private string _tempAsiceFilePath = string.Empty;
    private string _extractedTempAsiceFilePath = string.Empty;
    
    public async Task<bool> ValidateSignatures(IFormFile file, string? userId)
    {
        _tempAsicePath = Path.Combine(AppContext.BaseDirectory, "tempAsiceFiles");
        _tempAsiceFilePath = Path.Combine(_tempAsicePath, file.FileName + new Random().Next() + ".asice");

        using var stream = new FileStream(_tempAsiceFilePath, FileMode.Create);
        await file.CopyToAsync(stream);

        _extractedTempAsiceFilePath = Path.Combine(_tempAsicePath, "extracted");
        Directory.CreateDirectory(_extractedTempAsiceFilePath);

        ZipFile.ExtractToDirectory(_tempAsiceFilePath, _extractedTempAsiceFilePath);

        var hasSignature0 = File.Exists(Path.Combine(_extractedTempAsiceFilePath, "META-INF", "signatures0.xml"));
        var hasSignature1 = File.Exists(Path.Combine(_extractedTempAsiceFilePath, "META-INF", "signatures1.xml"));

        return (hasSignature0 && userId is null && !hasSignature1) ||
        (hasSignature0 && userId is not null && hasSignature1);
    }

    public bool ValidateContent(Contract contract)
    {
        var pdfFilePath = Directory.GetFiles(_extractedTempAsiceFilePath, "*.pdf", SearchOption.AllDirectories).FirstOrDefault();
        if (pdfFilePath is null)
        {
            return false;
        }
        string pdfText = string.Empty;
        using var pdfDocument = PdfDocument.Open(pdfFilePath);
        pdfText = string.Join("\n", pdfDocument.GetPages().Select(page => page.Text));

        string docxText = string.Empty;
        using var stream = new MemoryStream(contract.FileData);
        using var docxDocument = WordprocessingDocument.Open(stream, false);
        docxText = docxDocument.MainDocumentPart.Document.Body.InnerText;

        File.Delete(_tempAsiceFilePath);
        Directory.Delete(_extractedTempAsiceFilePath, true);

        return CompareContent(pdfText, docxText);
    }

    private static bool CompareContent(string text1, string text2)
    {
        static string Normalize(string text) => text.Trim().Replace("\r", "").Replace("\n", "").Replace(" ", "");
        return Normalize(text1) == Normalize(text2);
    }
}
