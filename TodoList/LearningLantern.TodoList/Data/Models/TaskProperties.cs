using System.ComponentModel.DataAnnotations;

namespace LearningLantern.TodoList.Data.Models;

public class TaskProperties
{
    [Required] [StringLength(450)] public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime DueDate { get; set; }
    public bool MyDay => StartDate.Date <= DateTime.Today && DateTime.Today <= DueDate.Date;
    public bool Completed { get; set; }
    public bool Important { get; set; }
}