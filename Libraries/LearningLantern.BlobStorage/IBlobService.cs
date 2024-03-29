using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;

namespace LearningLantern.AzureBlobStorage;

public interface IBlobService
{
    Task<BlobDownloadInfo?> DownloadBlobAsync(string name);
    Task<string> UploadBlobAsync(string name, IFormFile file);
    Task<bool> DeleteBlobAsync(string name);
}