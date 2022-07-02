using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace LearningLantern.AzureBlobStorage;

public class BlobService : IBlobService
{
    private readonly BlobServiceClient _blobServiceClient;
    private readonly string _containerName;
    private readonly ILogger<BlobService> _logger;

    public BlobService(string connectionString, string containerName, ILogger<BlobService> logger)
    {
        _blobServiceClient = new BlobServiceClient(connectionString);
        _containerName = containerName;
        _logger = logger;
    }

    public async Task<BlobDownloadInfo?> DownloadBlobAsync(string name)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
        var blobClient = containerClient.GetBlobClient(name);

        var exist = await blobClient.ExistsAsync();

        if (!exist.Value) return null;

        var response = await blobClient.DownloadAsync();
        return response.Value;
    }

    public async Task<bool> UploadBlobAsync(string name, IFormFile file)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);


        var blobClient = containerClient.GetBlobClient(name);

        var response = await containerClient.ExistsAsync();

        var result
            = await blobClient.UploadAsync(file.OpenReadStream(),
                new BlobHttpHeaders
                {
                    ContentType = file.ContentType
                });
        

        return result is not null && !result.GetRawResponse().IsError;
    }

    public async Task<bool> DeleteBlobAsync(string name)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
        var blobClient = containerClient.GetBlobClient(name);
        return await blobClient.DeleteIfExistsAsync();
    }
}