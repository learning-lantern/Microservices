using LearningLantern.Calendar.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace LearningLantern.Calendar.Data;

public class CalendarContext : DbContext, ICalendarContext
{
    public CalendarContext(DbContextOptions option) : base(option)
    {
    }


    public DbSet<EventModel> Events { get; set; }
}