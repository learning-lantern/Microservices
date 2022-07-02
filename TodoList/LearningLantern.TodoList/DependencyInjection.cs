using LearningLantern.Common.Extensions;
using LearningLantern.Common.Services;
using LearningLantern.TodoList.Data;
using LearningLantern.TodoList.Repositories;
using LearningLantern.TodoList.Utility;
using Microsoft.EntityFrameworkCore;

namespace LearningLantern.TodoList;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddDatabase();
        services.AddAuthenticationConfigurations();

        services.AddAutoMapper(typeof(MappingProfile));
        services.AddTransient<ITodoRepository, TodoRepository>();
        services.AddSingleton<ICurrentUserService, CurrentUserService>();
        services.AddHttpContextAccessor();
        return services;
    }

    public static IServiceCollection AddDatabase(this IServiceCollection services)
    {
        services.AddDbContext<ITodoContext, TodoContext>(builder =>
        {
            var myServerAddress = "learning-lantern.database.windows.net";
            var myUsername = "LearningLanternAdmin";
            var password = "TwajbuxAReMej9";
            var myDatabase = "TodoList";
            var connectionString =
                $"Server={myServerAddress};Database={myDatabase};User Id={myUsername};Password={password}";
            builder.UseSqlServer(connectionString);
        });
        return services;
    }
}