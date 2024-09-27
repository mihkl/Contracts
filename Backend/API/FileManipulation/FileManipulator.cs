using System.Text.RegularExpressions;
using API.Models;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace API.FileManipulation;

public static class FileManipulator
{
    public static Contract ParseDocxFile(IFormFile file)
    {
        if (file is null || file.Length == 0)
        {
            throw new ArgumentException("File is null or empty");
        }
        if (file.ContentType != "application/vnd.openxmlformats-officedocument.wordprocessingml.document")
        {
            throw new ArgumentException("File is not a valid docx file");
        }

        byte[] fileData;
        using (var memoryStream = new MemoryStream())
        {
            file.CopyTo(memoryStream);
            fileData = memoryStream.ToArray();
        }

        string docText;
        using (var stream = file.OpenReadStream())
        {
            using WordprocessingDocument doc = WordprocessingDocument.Open(stream, false);
            if (doc.MainDocumentPart?.Document?.Body is null)
            {
                throw new InvalidOperationException("The document is not in the expected format.");
            }
            docText = doc.MainDocumentPart.Document.Body.InnerText;
        }

        var fields = FindDynamicFields(docText);
        var response = new Contract
        {
            Name = file.FileName,
            FileData = fileData,
            Fields = fields
        };
        return response;
    }

    private static List<DynamicField> FindDynamicFields(string text)
    {
        var dynamicFields = new List<DynamicField>();
        foreach (var field in PlaceHolderMappings.Fields)
        {
            var match = Regex.Match(text, field.Value);
            if (match.Success)
            {
                dynamicFields.Add(new DynamicField
                {
                    Placeholder = field.Value,
                    Name = field.Key.Name,
                    Value = string.Empty,
                    Type = field.Key.Type
                });
            }
        }
        return dynamicFields;
    }

    public static byte[] ReplacePlaceholdersInDocument(Contract contract, List<DynamicFieldReplacement> replacements)
    {
    using MemoryStream memoryStream = new(contract.FileData);
    using (WordprocessingDocument wordDocument = WordprocessingDocument.Open(memoryStream, true))
    {
        var mainPart = wordDocument.MainDocumentPart;

        if (mainPart?.Document?.Body is null)
        {
            throw new InvalidOperationException("The document is not in the expected format.");
        }

        foreach (var field in replacements)
        {
            var placeholder = contract.Fields.FirstOrDefault(f => f.Name == field.Name)?.Placeholder;
            if (placeholder is null)
            {
                continue;
            }
            ReplaceText(mainPart, placeholder, field.Value);
        }

        mainPart.Document.Save();
    }
    memoryStream.Position = 0; 

    return memoryStream.ToArray();
    }

    private static void ReplaceText(MainDocumentPart mainPart, string placeholder, string newValue)
    {
        var document = mainPart.Document;
        var textElements = document.Descendants<Text>().Where(t => t.Text.Contains(placeholder)).ToList();

        foreach (var textElement in textElements)
        {
            textElement.Text = textElement.Text.Replace(placeholder, newValue);
        }
        document.Save();
    }
}
