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


    public async Task<Response<AddTaskResponse>> AddAsync(string userId, AddTaskDTO addTaskDTO)
    {
        var tmpTask = _mapper.Map<TaskModel>(addTaskDTO);
        tmpTask.UserId = userId;

        var task = await _context.Tasks.AddAsync(tmpTask);

        var result = await _context.SaveChangesAsync();

        return result == 0
            ? ResponseFactory.Fail<AddTaskResponse>()
            : ResponseFactory.Ok(new AddTaskResponse(task.Entity, addTaskDTO.TempId));
    }

    public async Task<Response<IEnumerable<TaskModel>>> GetAsync(string userId, string? list)
    {
        list = list?.ToLower();

        var query = list switch
        {
            "myday" => GetTasks(task => task.UserId == userId && task.MyDay),
            "completed" => GetTasks(task => task.UserId == userId && task.Completed),
            "important" => GetTasks(task => task.UserId == userId && task.Important),
            _ => GetTasks(task => task.UserId == userId)
        };

        return ResponseFactory.Ok<IEnumerable<TaskModel>>(await query.ToListAsync());
    }

    public async Task<Response> RemoveAsync(int taskId)
    {
        var task = await GetTasks(task => task.Id == taskId).FirstOrDefaultAsync();

        if (task == null) return ResponseFactory.Fail(ErrorsList.TaskNotFound(taskId));

        _context.Tasks.Remove(task);

        return await _context.SaveChangesAsync() != 0 ? ResponseFactory.Ok() : ResponseFactory.Fail();
    }

    public async Task<Response> UpdateAsync(int taskId, TaskProperties taskProperties)
    {
        var task = await GetTasks(task => task.Id == taskId).FirstOrDefaultAsync();

        if (task == null) return ResponseFactory.Fail(ErrorsList.TaskNotFound(taskId));

        task.Update(taskProperties);
        _context.Tasks.Update(task);

        return await _context.SaveChangesAsync() != 0 ? ResponseFactory.Ok() : ResponseFactory.Fail();
    }

    private IQueryable<TaskModel> GetTasks(Expression<Func<TaskModel, bool>> filter) => _context.Tasks.Where(filter);
}