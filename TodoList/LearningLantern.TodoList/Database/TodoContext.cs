using LearningLantern.Common.Models.TodoModels;
using Microsoft.EntityFrameworkCore;

namespace LearningLantern.TodoList.Database;

public class TodoContext : DbContext, ITodoContext
{
    public TodoContext(DbContextOptions option) : base(option)
    {
    }

    public DbSet<TaskModel> Tasks { get; set; } = null!;
}