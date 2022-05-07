using LearningLantern.Common.Models.TodoModels;
using LearningLantern.Common.Response;

namespace LearningLantern.TodoList.Repositories;

public interface ITodoRepository
{
    public Task<Response<TaskModel>> AddAsync(TaskDTO taskDTO);
    public Task<Response<IEnumerable<TaskModel>>> GetAsync(string userId, string? list);
    public Task<Response<TaskModel>> GetTaskByIdAsync(int taskId);
    public Task<Response> UpdateAsync(int taskId, UpdateTaskDTO updateTaskDTO);
    public Task<Response> RemoveAsync(int taskId);
}