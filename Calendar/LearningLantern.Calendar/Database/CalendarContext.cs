using LearningLantern.Common.Models.CalendarModels;
using Microsoft.EntityFrameworkCore;

namespace LearningLantern.Calendar.Database;

public class CalendarContext : DbContext, ICalendarContext
{
    public CalendarContext(DbContextOptions option) : base(option)
    {
    }


    public DbSet<EventModel> Events { get; set; }
}