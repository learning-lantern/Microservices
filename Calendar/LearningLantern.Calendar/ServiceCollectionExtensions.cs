using LearningLantern.Calendar.Database;
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
        services.AddDbContext<ICalendarContext, CalendarContext>(options =>
            options.UseInMemoryDatabase("Calendar"));
        return services;
    }
}