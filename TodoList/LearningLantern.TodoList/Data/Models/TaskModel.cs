using System.ComponentModel.DataAnnotations;

namespace LearningLantern.TodoList.Data.Models;

public class TaskModel : AddTaskDTO
{
    [Key]
    [Required]
    public int Id { get; set; }
    [Required]
    public string UserId { get; set; } = null!;

    public void Update(AddTaskDTO taskProperties)
    {
        Title = taskProperties.Title;
        DueDate = taskProperties.DueDate;
        Note = taskProperties.Note;
        MyDay = taskProperties.MyDay;
        Completed = taskProperties.Completed;
        Important = taskProperties.Important;
        Repeated = taskProperties.Repeated;
    }
}