using LearningLantern.Common;
using LearningLantern.Common.Response;
using LearningLantern.Common.Services;
using LearningLantern.TodoList.Data.Models;
using LearningLantern.TodoList.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearningLantern.TodoList.Controllers;

[Authorize]
[Route("/api/v1/[controller]")]
public class TasksController : ApiControllerBase
{
    private readonly ICurrentUserService _currentUserService;
    private readonly ITodoRepository _todoRepository;

    public TasksController(ITodoRepository todoRepository, ICurrentUserService currentUserService)
    {
        _todoRepository = todoRepository;
        _currentUserService = currentUserService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(Response<TaskDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Add([FromBody] TaskProperties task)
    {
        var userId = _currentUserService.UserId;
        var response = await _todoRepository.AddAsync(userId, task);
        return ResponseToIActionResult(response);
    }

    [HttpGet]
    [ProducesResponseType(typeof(Response<Dictionary<int, TaskDTO>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get([FromQuery] string? list)
    {
        var userId = _currentUserService.UserId;
        var response = await _todoRepository.GetListAsync(userId, list);
        return ResponseToIActionResult(response);
    }

    [HttpGet("{taskId:int}")]
    [ProducesResponseType(typeof(Response<Dictionary<int, TaskDTO>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById([FromBody] int taskId)
    {
        var userId = _currentUserService.UserId;
        var response = await _todoRepository.GetByIdAsync(userId, taskId);
        return ResponseToIActionResult(response);
    }

    [HttpPut("{taskId:int}")]
    [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update([FromRoute] int taskId, [FromBody] TaskProperties taskProperties)
    {
        var response = await _todoRepository.UpdateAsync(taskId, taskProperties);
        return ResponseToIActionResult(response);
    }

    [HttpDelete("{taskId:int}")]
    [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Remove([FromRoute] int taskId)
    {
        var response = await _todoRepository.RemoveAsync(taskId);
        return ResponseToIActionResult(response);
    }
}