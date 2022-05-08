using System.Linq.Expressions;
using AutoMapper;
using LearningLantern.Common.Models.TodoModels;
using LearningLantern.Common.Result;
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

    public async Task<Result<TaskModel>> AddAsync(TaskDTO taskDTO)
    {
        var tmpTask = _mapper.Map<TaskModel>(taskDTO);

        var task = await _context.Tasks.AddAsync(tmpTask);

        var result = await _context.SaveChangesAsync();

        if (result == 0) throw new CreateTaskFailedException();

        return ResultFactory.Ok(task.Entity);
    }

    public async Task<Result<IEnumerable<TaskModel>>> GetAsync(string userId, string? list)
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
        return ResultFactory.Ok(result);
    }

    public async Task<Result<TaskModel>> GetTaskByIdAsync(int taskId)
    {
        var task = await GetTasks(task => task.Id == taskId).FirstOrDefaultAsync();

        if (task == null) throw new TaskNotFoundException();

        return ResultFactory.Ok(task);
    }

    public async Task<Result> UpdateAsync(int taskId, UpdateTaskDTO updateTaskDTO)
    {
        var task = await GetTasks(task => task.Id == taskId).FirstOrDefaultAsync();
        if (task == null) throw new TaskNotFoundException();

        task.Update(updateTaskDTO);
        _context.Tasks.Update(task);

        var result = await _context.SaveChangesAsync() != 0;
        return result ? ResultFactory.Ok() : ResultFactory.Fail(default);
    }

    public async Task<Result> RemoveAsync(int taskId)
    {
        var task = await GetTasks(task => task.Id == taskId).FirstOrDefaultAsync();

        if (task == null) return ResultFactory.Ok();
        ;

        _context.Tasks.Remove(task);

        var result = await _context.SaveChangesAsync() != 0;
        return result ? ResultFactory.Ok() : ResultFactory.Fail(default);
    }

    private IQueryable<TaskModel> GetTasks(Expression<Func<TaskModel, bool>> filter) => _context.Tasks.Where(filter);
}