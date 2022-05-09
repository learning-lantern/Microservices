namespace LearningLantern.TodoList.Data.Models;

public class AddTaskResponse
{
    public AddTaskResponse(TaskModel task, string tempId)
    {
        Task = task;
        TempId = tempId;
    }

    public TaskModel Task { get; }
    public string TempId { get; }
}