using Microsoft.AspNetCore.StaticFiles;

namespace LearningLantern.AzureBlobStorage;

public static class Extensions
{
    private static readonly FileExtensionContentTypeProvider provider = new();

    public static string GetContentType(this string fileName)
    {
        if (!provider.TryGetContentType(fileName, out var contentType)) contentType = "application/octet-stream";
        return contentType;
    }
}