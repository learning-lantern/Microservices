using LearningLantern.Calendar.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace LearningLantern.Calendar.Data;

public interface ICalendarContext
{
    public DbSet<EventModel> Events { get; set; }
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}