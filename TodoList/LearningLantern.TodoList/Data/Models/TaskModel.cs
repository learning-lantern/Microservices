using System.ComponentModel.DataAnnotations;

namespace LearningLantern.TodoList.Data.Models;

public class TaskModel : TaskDTO
{
    [Required] public string UserId { get; set; }

    public void Update(TaskProperties taskProperties)
    {
        Title = taskProperties.Title;
        StartDate = taskProperties.StartDate;
        DueDate = taskProperties.DueDate;
        Description = taskProperties.Description;
        Completed = taskProperties.Completed;
        Important = taskProperties.Important;
    }
}