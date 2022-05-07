using LearningLantern.Common.Models.TodoModels;
using Microsoft.EntityFrameworkCore;

namespace LearningLantern.TodoList.Database;

public interface ITodoContext
{
    public DbSet<TaskModel> Tasks { get; set; }
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}