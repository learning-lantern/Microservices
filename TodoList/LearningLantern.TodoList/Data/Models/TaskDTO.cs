using System.ComponentModel.DataAnnotations;

namespace LearningLantern.TodoList.Data.Models;

public class TaskDTO : TaskProperties
{
    [Required] [Key] public int Id { get; set; }
}