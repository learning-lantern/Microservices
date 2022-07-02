using LearningLantern.AzureBlobStorage;
using LearningLantern.Common.Extensions;
using LearningLantern.Common.Services;
using LearningLantern.Video.Data;
using LearningLantern.Video.Repositories;
using LearningLantern.Video.Utility;
using Microsoft.EntityFrameworkCore;

namespace LearningLantern.Video;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddDatabase();
        services.AddAuthenticationConfigurations();

        services.AddHttpContextAccessor();
        services.AddAutoMapper(typeof(MappingProfile));

        services.AddBlobService("videos");

        services.AddTransient<IVideoRepository, VideoRepository>();
        services.AddSingleton<ICurrentUserService, CurrentUserService>();

        return services;
    }

    public static IServiceCollection AddDatabase(this IServiceCollection services)
    {
        services.AddDbContext<IVideoContext, VideoContext>(builder =>
        {
            var myServerAddress = "learning-lantern.database.windows.net";
            var myUsername = "LearningLanternAdmin";
            var password = "TwajbuxAReMej9";
            var myDatabase = "Videos";
            var connectionString =
                $"Server={myServerAddress};Database={myDatabase};User Id={myUsername};Password={password}";
            builder.UseSqlServer(connectionString);
        });
        
        return services;
    }
}