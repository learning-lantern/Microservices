using LearningLantern.Common.Response;
using LearningLantern.TodoList.Data.Models;

namespace LearningLantern.TodoList.Repositories;

public interface ITodoRepository
{
    public Task<Response<TaskDTO>> AddAsync(string userId, TaskProperties task);
    public Task<Response<IEnumerable<TaskDTO>>> GetByIdAsync(string userId, string? list);
    public Task<Response<TaskDTO>> GetByIdAsync(int taskId);
    public Task<Response> UpdateAsync(int taskId, TaskProperties updateTaskDTO);
    public Task<Response> RemoveAsync(int taskId);
}