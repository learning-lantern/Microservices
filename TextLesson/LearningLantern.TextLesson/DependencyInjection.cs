using LearningLantern.AzureBlobStorage;
using LearningLantern.Common.Extensions;
using LearningLantern.Common.Services;
using LearningLantern.TextLesson.Data;
using LearningLantern.TextLesson.Repositories;
using LearningLantern.TextLesson.Utility;
using Microsoft.EntityFrameworkCore;

namespace LearningLantern.TextLesson;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddDatabase();
        services.AddAuthenticationConfigurations();

        services.AddHttpContextAccessor();
        services.AddAutoMapper(typeof(MappingProfile));

        services.AddBlobService("textlessons");

        services.AddTransient<ITextLessonRepository, TextLessonRepository>();
        services.AddSingleton<ICurrentUserService, CurrentUserService>();

        return services;
    }

    public static IServiceCollection AddDatabase(this IServiceCollection services)
    {
        services.AddDbContext<ITextLessonContext, TextLessonContext>(builder =>
        {
            var myServerAddress = "learning-lantern.database.windows.net";
            var myUsername = "LearningLanternAdmin";
            var password = "TwajbuxAReMej9";
            var myDatabase = "TextLessons";
            var connectionString =
                $"Server={myServerAddress};Database={myDatabase};User Id={myUsername};Password={password}";
            builder.UseSqlServer(connectionString);
        });
        
        return services;
    }
}