using LearningLantern.Common.DependencyInjection;
using LearningLantern.TodoList.Data;
using LearningLantern.TodoList.Repositories;
using LearningLantern.TodoList.Services;
using LearningLantern.TodoList.Utility;
using Microsoft.EntityFrameworkCore;

namespace LearningLantern.TodoList;

public static class ServiceCollectionExtensions
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
            var myDatabase = "TodoList";
            var myUsername = "learinglanternadmin";
            var password = "z8ZkHzdWw8a79B";
            var connectionString =
                $"Server={myServerAddress};Database={myDatabase};User Id={myUsername};Password={password}";
            builder.UseSqlServer(connectionString);
        });
        return services;
    }
}