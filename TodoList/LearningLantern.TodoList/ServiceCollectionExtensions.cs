using LearningLantern.TodoList.Database;
using LearningLantern.TodoList.Repositories;
using LearningLantern.TodoList.Utility;
using Microsoft.EntityFrameworkCore;

namespace LearningLantern.TodoList;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddDatabase();
        services.AddAutoMapper(typeof(MappingProfile));
        services.AddTransient<ITodoRepository, TodoRepository>();
        return services;
    }

    public static IServiceCollection AddDatabase(this IServiceCollection services)
    {
        var database = "TodoList";
        services.AddDbContext<ITodoContext, TodoContext>(options =>
            options.UseInMemoryDatabase(database));
        return services;
    }
}