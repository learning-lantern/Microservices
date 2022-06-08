using System.Threading;
using System.Threading.Tasks;
using LearningLantern.TodoList.Data;
using LearningLantern.TodoList.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace LearningLantern.TodoList.Tests.Helpers;

public class TodoContextMock : DbContext, ITodoContext
{
    public TodoContextMock(DbContextOptions option) : base(option)
    {
    }

    public int CountCalls { get; internal set; }
    public DbSet<TaskModel> Tasks { get; set; }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        CountCalls++;
        return base.SaveChangesAsync(cancellationToken);
    }
}