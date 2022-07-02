using LearningLantern.Common.Response;
using LearningLantern.TodoList.Data.Models;

namespace LearningLantern.TodoList.Repositories;

public interface ITodoRepository
{
    public Task<Response<TaskDTO>> AddAsync(string userId, TaskProperties task);
    public Task<Response<Dictionary<int, TaskDTO>>> GetListAsync(string userId, string? list);
    public Task<Response<TaskDTO>> GetByIdAsync(string userId, int taskId);
    public Task<Response<TaskDTO>> UpdateAsync(int taskId, TaskProperties updateTaskDTO);
    public Task<Response<int>> RemoveAsync(int taskId);
}