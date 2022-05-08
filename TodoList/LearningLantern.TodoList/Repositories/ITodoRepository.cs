using LearningLantern.Common.Models.TodoModels;
using LearningLantern.Common.Result;

namespace LearningLantern.TodoList.Repositories;

public interface ITodoRepository
{
    public Task<Result<TaskModel>> AddAsync(TaskDTO taskDTO);
    public Task<Result<IEnumerable<TaskModel>>> GetAsync(string userId, string? list);
    public Task<Result<TaskModel>> GetTaskByIdAsync(int taskId);
    public Task<Result> UpdateAsync(int taskId, UpdateTaskDTO updateTaskDTO);
    public Task<Result> RemoveAsync(int taskId);
}