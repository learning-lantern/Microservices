using System.ComponentModel.DataAnnotations;

namespace LearningLantern.TodoList.Data.Models;

public class TaskModel : TaskDTO
{
    [Required] [Key] public int Id { get; set; }


    public void Update(UpdateTaskDTO updateTaskDTO)
    {
        Title = updateTaskDTO.Title;
        DueDate = updateTaskDTO.DueDate;
        Note = updateTaskDTO.Note;
        MyDay = updateTaskDTO.MyDay;
        Completed = updateTaskDTO.Completed;
        Important = updateTaskDTO.Important;
        Repeated = updateTaskDTO.Repeated;
    }
}