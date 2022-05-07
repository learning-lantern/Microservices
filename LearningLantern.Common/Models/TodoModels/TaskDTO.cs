using System.ComponentModel.DataAnnotations;

namespace LearningLantern.Common.Models.TodoModels;

public class TaskDTO : UpdateTaskDTO
{
    [Required] public string UserId { get; set; }
}