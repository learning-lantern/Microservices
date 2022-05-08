using LearningLantern.Common.Response;
using LearningLantern.TodoList.Data.Models;

namespace LearningLantern.TodoList.Repositories;

public interface ITodoRepository
{
    public Task<Response<TaskModel>> AddAsync(TaskDTO taskDTO);
    public Task<Response<IEnumerable<TaskModel>>> GetAsync(string userId, string? list);
    public Task<Response<TaskModel>> GetTaskByIdAsync(int taskId);
    public Task<Response> UpdateAsync(int taskId, UpdateTaskDTO updateTaskDTO);
    public Task<Response> RemoveAsync(int taskId);
}