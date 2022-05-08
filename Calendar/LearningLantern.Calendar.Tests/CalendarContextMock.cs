using System.Threading;
using System.Threading.Tasks;
using LearningLantern.Calendar.Data;
using LearningLantern.Calendar.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace LearningLantern.Calendar.Tests;

public class CalendarContextMock : DbContext, ICalendarContext
{
    public CalendarContextMock(DbContextOptions option) : base(option)
    {
    }

    public int CountCalls { get; internal set; }


    public DbSet<EventModel> Events { get; set; }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        CountCalls++;
        return base.SaveChangesAsync(cancellationToken);
    }
}