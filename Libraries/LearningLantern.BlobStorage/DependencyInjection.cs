using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace LearningLantern.AzureBlobStorage;

public static class DependencyInjection
{
    private static readonly string connectionString = "DefaultEndpointsProtocol=https;AccountName=learninglantern;AccountKey=WKgvAxchhiCJw/TnI8DqzEuuEITQ7DAiQzjrWwsqytsdaRKFBd4VSI6JI9Rz3Jxbtj6LVbhoA9Qd+AStd18WWA==;EndpointSuffix=core.windows.net";
    
    public static IServiceCollection AddBlobService(
        this IServiceCollection services, string containerName)
    {
        services.AddSingleton<IBlobService, BlobService>(
            provider => new BlobService(connectionString, containerName, provider.GetService<ILogger<BlobService>>())
        );
        return services;
    }
}