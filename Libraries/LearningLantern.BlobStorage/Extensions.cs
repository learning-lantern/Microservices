using Microsoft.AspNetCore.StaticFiles;

namespace LearningLantern.AzureBlobStorage;

public static class Extensions
{
    private static readonly FileExtensionContentTypeProvider Provider = new();

    public static string GetContentType(this string fileName)
    {
        if (!Provider.TryGetContentType(fileName, out var contentType)) contentType = "application/octet-stream";
        return contentType;
    }
}