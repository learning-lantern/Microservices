using LearningLantern.TodoList.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace LearningLantern.TodoList.Data;

public interface ITodoContext
{
    public DbSet<TaskModel> Tasks { get; }
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}