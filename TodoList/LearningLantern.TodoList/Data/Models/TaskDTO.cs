using System.ComponentModel.DataAnnotations;

namespace LearningLantern.TodoList.Data.Models;

public class TaskDTO : UpdateTaskDTO
{
    [Required] public string UserId { get; set; }
}