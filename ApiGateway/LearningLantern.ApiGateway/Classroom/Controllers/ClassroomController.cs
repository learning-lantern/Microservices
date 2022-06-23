using LearningLantern.ApiGateway.Classroom.DTOs;
using LearningLantern.ApiGateway.Classroom.Repositories;
using LearningLantern.Common;
using LearningLantern.Common.Responses;
using LearningLantern.Common.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearningLantern.ApiGateway.Classroom.Controllers;

[Route("api/[controller]/[action]")]
[Authorize]
public class ClassroomController : ApiControllerBase
{
    private readonly IClassroomRepository _classroomRepository;
    private readonly ICurrentUserService _currentUserService;

    public ClassroomController(IClassroomRepository classroomRepository, ICurrentUserService currentUserService)
    {
        _classroomRepository = classroomRepository;
        _currentUserService = currentUserService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(Response<IEnumerable<ClassroomDTO>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get()
    {
        var userId = _currentUserService.UserId ?? string.Empty;

        var response = await _classroomRepository.GetAsync(userId);

        return ResponseToIActionResult(response);
    }

    [HttpGet]
    [Authorize(Roles = LearningLanternRoles.UniversityAdmin)]
    [ProducesResponseType(typeof(Response<IEnumerable<ClassroomDTO>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var response = await _classroomRepository.GetAllAsync();

        return ResponseToIActionResult(response);
    }

    [HttpPost]
    [Authorize(Roles = LearningLanternRoles.UniversityAdmin)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Add([FromBody] AddClassroomDTO addClassroomDTO)
    {
        var response = await _classroomRepository.AddAsync(addClassroomDTO);

        return ResponseToIActionResult(response);
    }

    [HttpPost]
    [Authorize(Roles = LearningLanternRoles.UniversityAdmin)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddUser([FromQuery] int classroomId, [FromQuery] string userId)
    {
        var response = await _classroomRepository.AddUserAsync(classroomId, userId);
        return ResponseToIActionResult(response);
    }

    [HttpPut]
    [Authorize(Roles = LearningLanternRoles.UniversityAdmin)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update([FromBody] ClassroomDTO classroomDTO)
    {
        var response = await _classroomRepository.UpdateAsync(classroomDTO);

        return ResponseToIActionResult(response);
    }

    [HttpDelete]
    [Authorize(Roles = LearningLanternRoles.UniversityAdmin)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RemoveUser([FromQuery] int classroomId, [FromQuery] string userId)
    {
        var response = await _classroomRepository.RemoveUserAsync(classroomId, userId);

        return ResponseToIActionResult(response);
    }

    [HttpDelete]
    [Authorize(Roles = LearningLanternRoles.UniversityAdmin)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Remove([FromQuery] int classroomId)
    {
        var response = await _classroomRepository.RemoveAsync(classroomId);

        return ResponseToIActionResult(response);
    }
}