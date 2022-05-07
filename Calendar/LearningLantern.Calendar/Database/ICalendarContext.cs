using LearningLantern.Common.Models.CalendarModels;
using Microsoft.EntityFrameworkCore;

namespace LearningLantern.Calendar.Database;

public interface ICalendarContext
{
    public DbSet<EventModel> Events { get; set; }
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}