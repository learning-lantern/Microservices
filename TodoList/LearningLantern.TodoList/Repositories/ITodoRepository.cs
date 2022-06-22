using LearningLantern.Common.Responses;
using LearningLantern.TodoList.Data.Models;

namespace LearningLantern.TodoList.Repositories;

public interface ITodoRepository
{
    public Task<Response<TaskModel>> AddAsync(string userId, AddTaskDTO task);
    public Task<Response<IEnumerable<TaskModel>>> GetAsync(string userId, string? list);
    public Task<Response> UpdateAsync(TaskModel taskModel);
    public Task<Response> RemoveAsync(int taskId);
}