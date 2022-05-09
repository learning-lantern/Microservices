using LearningLantern.Common.Response;
using LearningLantern.TodoList.Data.Models;

namespace LearningLantern.TodoList.Repositories;

public interface ITodoRepository
{
    public Task<Response<AddTaskResponse>> AddAsync(string userId, AddTaskDTO task);
    public Task<Response<IEnumerable<TaskModel>>> GetAsync(string userId, string? list);
    public Task<Response> UpdateAsync(int taskId, TaskProperties updateTaskDTO);
    public Task<Response> RemoveAsync(int taskId);
}