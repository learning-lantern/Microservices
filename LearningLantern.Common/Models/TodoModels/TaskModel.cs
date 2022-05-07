namespace LearningLantern.Common.Models.TodoModels;

public class TaskModel : TaskDTO, IEntity
{
    public int Id { get; set; }


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