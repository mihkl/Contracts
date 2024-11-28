
using API.FileManipulation;
using API.Models;

namespace API.Data;

public static class DataBaseSeeder
{
    public static async Task<Template> TemplateFromFileAsync(string path)
    {
        var projectRoot = Directory.GetParent(AppContext.BaseDirectory)?.FullName;
        var filePath = Path.Combine(projectRoot!, "TestFiles", path);

        await using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        var file = new FormFile(fileStream, 0, fileStream.Length, path, path)
        {
            Headers = new HeaderDictionary(),
            ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document"
        };
        return FileManipulator.ParseDocxFile(file, out _)!;
    }
}
