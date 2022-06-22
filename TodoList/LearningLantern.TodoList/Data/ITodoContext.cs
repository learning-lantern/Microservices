using LearningLantern.TodoList.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace LearningLantern.TodoList.Data;

public interface ITodoContext
{
    DbSet<TaskModel> Tasks { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}