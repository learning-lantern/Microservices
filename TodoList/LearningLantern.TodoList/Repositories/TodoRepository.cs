using System.Linq.Expressions;
using AutoMapper;
using LearningLantern.Common.Models.TodoModels;
using LearningLantern.Common.Response;
using LearningLantern.TodoList.Database;
using LearningLantern.TodoList.Exceptions;
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

    public async Task<Response<TaskModel>> AddAsync(TaskDTO taskDTO)
    {
        var tmpTask = _mapper.Map<TaskModel>(taskDTO);

        var task = await _context.Tasks.AddAsync(tmpTask);

        var result = await _context.SaveChangesAsync();

        if (result == 0) throw new CreateTaskFailedException();

        return ResponseFactory.Ok(task.Entity);
    }

    public async Task<Response<IEnumerable<TaskModel>>> GetAsync(string userId, string? list)
    {
        IQueryable<TaskModel> query;

        switch (list)
        {
            case "myDay":
                query = GetTasks(task => task.UserId == userId && task.MyDay);
                break;
            case "Completed":
                query = GetTasks(task => task.UserId == userId && task.Completed);
                break;
            case "Important":
                query = GetTasks(task => task.UserId == userId && task.Important);
                break;
            default:
                query = GetTasks(task => task.UserId == userId);
                break;
        }

        IEnumerable<TaskModel> result = await query.ToListAsync();
        return ResponseFactory.Ok(result);
    }

    public async Task<Response<TaskModel>> GetTaskByIdAsync(int taskId)
    {
        var task = await GetTasks(task => task.Id == taskId).FirstOrDefaultAsync();

        if (task == null) throw new TaskNotFoundException();

        return ResponseFactory.Ok(task);
    }

    public async Task<Response> UpdateAsync(int taskId, UpdateTaskDTO updateTaskDTO)
    {
        var task = await GetTasks(task => task.Id == taskId).FirstOrDefaultAsync();
        if (task == null) throw new TaskNotFoundException();

        task.Update(updateTaskDTO);
        _context.Tasks.Update(task);

        var result = await _context.SaveChangesAsync() != 0;
        return result ? ResponseFactory.Ok() : ResponseFactory.Fail(default);
    }

    public async Task<Response> RemoveAsync(int taskId)
    {
        var task = await GetTasks(task => task.Id == taskId).FirstOrDefaultAsync();

        if (task == null) return ResponseFactory.Ok();
        ;

        _context.Tasks.Remove(task);

        var result = await _context.SaveChangesAsync() != 0;
        return result ? ResponseFactory.Ok() : ResponseFactory.Fail(default);
    }

    private IQueryable<TaskModel> GetTasks(Expression<Func<TaskModel, bool>> filter) => _context.Tasks.Where(filter);
}