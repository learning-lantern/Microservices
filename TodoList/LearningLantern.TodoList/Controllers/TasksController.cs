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
    [ProducesResponseType(typeof(TaskDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IEnumerable<Error>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Add([FromBody] TaskProperties task)
    {
        var userId = _currentUserService.UserId;
        var response = await _todoRepository.AddAsync(userId, task);
        return ResponseToIActionResult(response);
    }

    [AllowAnonymous]
    [HttpGet("{userId}")]
    public async Task<IActionResult> GetUser([FromRoute] string userId)
    {
        var response = await _todoRepository.GetListAsync(userId, null);
        return ResponseToIActionResult(response);
    }

    [HttpGet]
    [ProducesResponseType(typeof(Dictionary<int, TaskDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IEnumerable<Error>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Get([FromQuery] string? list)
    {
        var userId = _currentUserService.UserId;
        var response = (await _todoRepository.GetDictionaryAsync(userId, list));
        return ResponseToIActionResult(response);
    }

    [HttpGet("{taskId:int}")]
    [ProducesResponseType(typeof(TaskDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IEnumerable<Error>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetById([FromRoute] int taskId)
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