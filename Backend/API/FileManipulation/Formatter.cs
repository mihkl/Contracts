using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace API.FileManipulation;

public static class Formatter
{
    public static byte[] FormatTextBody(byte[] inputBytes)
    {
        using MemoryStream memoryStream = new();

        memoryStream.Write(inputBytes, 0, inputBytes.Length);
        memoryStream.Position = 0;

        using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(memoryStream, true))
        {
            var body = wordDoc.MainDocumentPart!.Document.Body!;

            foreach (var paragraph in body.Elements<Paragraph>())
            {
                var runs = paragraph.Elements<Run>().ToList();

                if (runs.Count > 1)
                {
                    var newRun = new Run();

                    string combinedText = string.Join("", runs.Select(run => run.Elements<Text>()
                                                                                .FirstOrDefault()?.Text ?? ""));

                    var newText = new Text(combinedText)
                    {
                        Space = SpaceProcessingModeValues.Preserve
                    };

                    newRun.Append(newText);

                    paragraph.RemoveAllChildren<Run>();
                    paragraph.Append(newRun);
                }
            }
            wordDoc.MainDocumentPart.Document.Save();
            memoryStream.Position = 0;
        }
        return memoryStream.ToArray();
    }
}

