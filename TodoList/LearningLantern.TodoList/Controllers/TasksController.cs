using LearningLantern.Common;
using LearningLantern.Common.Models.TodoModels;
using LearningLantern.Common.Response;
using LearningLantern.TodoList.Exceptions;
using LearningLantern.TodoList.Repositories;
using LearningLantern.TodoList.Utility;
using Microsoft.AspNetCore.Mvc;

namespace LearningLantern.TodoList.Controllers;

[ApiController]
[Route("/api/v1/[controller]")]
public class TasksController : ControllerBase
{
    private readonly ITodoRepository _todoRepository;

    public TasksController(ITodoRepository todoRepository)
    {
        _todoRepository = todoRepository;
    }

    [HttpPost]
    [ProducesResponseType(typeof(Response<TaskModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Add([FromBody] TaskDTO taskDTO)
    {
        try
        {
            var task = await _todoRepository.AddAsync(taskDTO);
            return Ok(task);
        }
        catch (Exception)
        {
            return BadRequest();
        }
    }

    [HttpGet]
    [ProducesResponseType(typeof(Response<IEnumerable<TaskModel>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get([FromQuery] string userId, [FromQuery] string? list)
    {
        var response = await _todoRepository.GetAsync(userId, list);
        return Ok(response.ToJsonStringContent());
    }

    [HttpGet("{taskId:int}")]
    [ProducesResponseType(typeof(Response<TaskModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response<TaskModel>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTask([FromRoute] int taskId)
    {
        try
        {
            var response = await _todoRepository.GetTaskByIdAsync(taskId);
            return Ok(response.ToJsonStringContent());
        }
        catch (TaskNotFoundException)
        {
            var response = ResponseFactory.Fail(ErrorsList.TaskNotFound(taskId));
            return NotFound(response.ToJsonStringContent());
        }
    }

    [HttpPut("{taskId:int}")]
    [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update([FromRoute] int taskId, [FromBody] UpdateTaskDTO updateTaskDTO)
    {
        var response = await _todoRepository.UpdateAsync(taskId, updateTaskDTO);
        if (response.Succeeded) return Ok(response);
        return NotFound(response.ToJsonStringContent());
    }

    [HttpDelete("{taskId:int}")]
    [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Remove([FromRoute] int taskId)
    {
        var response = await _todoRepository.RemoveAsync(taskId);

        if (response.Succeeded) return Ok(response);

        return BadRequest(response);
    }
}