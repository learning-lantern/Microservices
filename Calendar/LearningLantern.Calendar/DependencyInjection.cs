using LearningLantern.Calendar.Data;
using LearningLantern.Calendar.Repositories;
using LearningLantern.Calendar.Utility;
using Microsoft.EntityFrameworkCore;

namespace LearningLantern.Calendar;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddDatabase();
        services.AddAutoMapper(typeof(MappingProfile));
        services.AddTransient<ICalendarRepository, CalendarRepository>();
        return services;
    }

    public static IServiceCollection AddDatabase(this IServiceCollection services)
    {
        services.AddDbContext<ICalendarContext, CalendarContext>(builder =>
        {
            var myServerAddress = "learning-lantern.database.windows.net";
            var myUsername = "LearningLanternAdmin";
            var password = "TwajbuxAReMej9";
            var myDatabase = "Calendar";
            var connectionString =
                $"Server={myServerAddress};Database={myDatabase};User Id={myUsername};Password={password}";
            builder.UseSqlServer(connectionString);
        });
        return services;
    }
}