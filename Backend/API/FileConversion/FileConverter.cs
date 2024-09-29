namespace API.FileConversion
{
    public static class FileConverter
    {
        public static void ConvertDocxToPdf(string fileInputPath, string fileOutputPath)
        {
            var converter = new DocXToPdfConverter.ReportGenerator();

            converter.Convert(fileInputPath, fileOutputPath);
        }
    }
}