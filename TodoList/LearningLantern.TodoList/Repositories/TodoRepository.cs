using System.Linq.Expressions;
using AutoMapper;
using LearningLantern.Common.Response;
using LearningLantern.TodoList.Data;
using LearningLantern.TodoList.Data.Models;
using LearningLantern.TodoList.Utility;
using Microsoft.EntityFrameworkCore;

namespace LearningLantern.TodoList.Repositories;

public class TodoRepository : ITodoRepository
{
    private readonly ITodoContext _context;
    private readonly IMapper _mapper;

    public TodoRepository(ITodoContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }


    public async Task<Response<TaskDTO>> AddAsync(string userId, TaskProperties task)
    {
        var tmpTask = _mapper.Map<TaskModel>(task);
        tmpTask.UserId = userId;

        var taskModel = await _context.Tasks.AddAsync(tmpTask);

        var result = await _context.SaveChangesAsync();

        return result == 0
            ? ResponseFactory.Fail<TaskDTO>()
            : ResponseFactory.Ok(_mapper.Map<TaskDTO>(taskModel.Entity));
    }

    public async Task<Response<Dictionary<int, TaskDTO>>> GetListAsync(string userId, string? list)
    {
        list = list?.ToLower();
        var query = await (list switch
            {
                "myday" => GetTasks(task => task.UserId == userId && task.MyDay),
                "completed" => GetTasks(task => task.UserId == userId && task.Completed),
                "important" => GetTasks(task => task.UserId == userId && task.Important),
                _ => GetTasks(task => task.UserId == userId)
            })
            .Select(task => _mapper.Map<TaskDTO>(task)).ToDictionaryAsync(task => task.Id);

        return ResponseFactory.Ok(query);
    }

    public async Task<Response<TaskDTO>> GetByIdAsync(string userId, int taskId)
    {
        var task = await GetTasks(task => task.Id == taskId && task.UserId == userId)
            .Select(task => _mapper.Map<TaskDTO>(task)).FirstOrDefaultAsync();
        return task is null ? ResponseFactory.Fail<TaskDTO>(ErrorsList.TaskNotFound(taskId)) : ResponseFactory.Ok(task);
    }

    public async Task<Response<int>> RemoveAsync(int taskId)
    {
        var task = await GetTasks(task => task.Id == taskId).FirstOrDefaultAsync();

        if (task == null) return ResponseFactory.Fail<int>(ErrorsList.TaskNotFound(taskId));

        _context.Tasks.Remove(task);

        return await _context.SaveChangesAsync() != 0 ? ResponseFactory.Ok(taskId) : ResponseFactory.Fail<int>();
    }

    public async Task<Response<TaskDTO>> UpdateAsync(int taskId, TaskProperties taskProperties)
    {
        var task = await GetTasks(task => task.Id == taskId).FirstOrDefaultAsync();

        if (task == null) return ResponseFactory.Fail<TaskDTO>(ErrorsList.TaskNotFound(taskId));

        task.Update(taskProperties);
        var updatedTask = _context.Tasks.Update(task);

        return await _context.SaveChangesAsync() != 0
            ? ResponseFactory.Ok(_mapper.Map<TaskDTO>(updatedTask))
            : ResponseFactory.Fail<TaskDTO>();
    }

    private IQueryable<TaskModel> GetTasks(Expression<Func<TaskModel, bool>> filter) => _context.Tasks.Where(filter);
}