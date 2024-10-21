using System.Text.RegularExpressions;
using API.Models;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace API.FileManipulation;

public static class FileManipulator
{
    public static Template? ParseDocxFile(IFormFile file, out string exception)
    {
        exception = string.Empty;
        if (file is null || file.Length == 0)
        {
            exception = "File is null or empty";
            return null;
        }
        if (file.ContentType != "application/vnd.openxmlformats-officedocument.wordprocessingml.document")
        {
            exception = "File is not a valid docx file";
            return null;
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
                exception = "The document is not in the expected format.";
                return null;
            }
            docText = doc.MainDocumentPart.Document.Body.InnerText;
        }

        var fields = FindTemplateDynamicFields(docText);
        var response = new Template
        {
            Name = file.FileName,
            FileData = fileData,
            Fields = fields
        };
        return response;
    }

    private static List<TemplateDynamicField> FindTemplateDynamicFields(string text)
    {
        var dynamicFields = new List<TemplateDynamicField>();
        foreach (var field in PlaceHolderMappings.Fields)
        {
            var match = Regex.Match(text, field.Value);
            if (match.Success)
            {
                dynamicFields.Add(new TemplateDynamicField
                {
                    Placeholder = field.Value,
                    Name = field.Key.Name,
                    Type = field.Key.Type
                });
            }
        }
        return dynamicFields;
    }

    public static byte[] ReplaceContractPlaceholders(Contract contract, List<ContractDynamicFieldReplacement> replacements)
    {
    using MemoryStream memoryStream = new(contract.FileData);
    using (WordprocessingDocument wordDocument = WordprocessingDocument.Open(memoryStream, true))
    {
        var mainPart = wordDocument.MainDocumentPart;

        foreach (var field in replacements)
        {
            var placeholder = contract.Fields.FirstOrDefault(f => f.Name == field.Name)?.Placeholder;
            if (placeholder is null)
            {
                continue;
            }
            ReplaceText(mainPart!, placeholder, field.Value);
        }

        mainPart!.Document.Save();
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
