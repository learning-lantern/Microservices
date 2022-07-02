using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace LearningLantern.AzureBlobStorage;

public static class DependencyInjection
{
    public static IServiceCollection AddBlobService(
        this IServiceCollection services, string connectionString, string containerName)
    {
        services.AddSingleton<IBlobService, BlobService>(
            provider => new BlobService(connectionString, containerName, provider.GetService<ILogger<BlobService>>())
        );
        return services;
    }
}