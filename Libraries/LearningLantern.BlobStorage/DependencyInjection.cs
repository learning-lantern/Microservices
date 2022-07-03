using Azure.Core.Pipeline;
using Azure.Storage.Blobs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace LearningLantern.AzureBlobStorage;

public static class DependencyInjection
{
    private const string ConnectionString =
        "DefaultEndpointsProtocol=https;AccountName=learninglantern;AccountKey=WKgvAxchhiCJw/TnI8DqzEuuEITQ7DAiQzjrWwsqytsdaRKFBd4VSI6JI9Rz3Jxbtj6LVbhoA9Qd+AStd18WWA==;EndpointSuffix=core.windows.net";

    public static IServiceCollection AddBlobService(
        this IServiceCollection services, string containerName)
    {
        var options = new BlobClientOptions
        {
            Transport = new HttpClientTransport(new HttpClient {Timeout = Timeout.InfiniteTimeSpan}),
            Retry = {NetworkTimeout = Timeout.InfiniteTimeSpan}
        };
        var blobClient = new BlobServiceClient(ConnectionString, options);

        services.AddSingleton<IBlobService, BlobService>(
            provider => new BlobService(blobClient, containerName, provider.GetService<ILogger<BlobService>>())
        );
        return services;
    }
}