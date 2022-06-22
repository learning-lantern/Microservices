using LearningLantern.TodoList.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace LearningLantern.TodoList.Data;

public class TodoContext : DbContext, ITodoContext
{
    public TodoContext(DbContextOptions option) : base(option) { }
    
    public DbSet<TaskModel> Tasks { get; set; } = null!;
}