using System.Security.Claims;
using LearningLantern.ApiGateway.Classroom.DTOs;
using LearningLantern.ApiGateway.Classroom.Repositories;
using LearningLantern.ApiGateway.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace LearningLantern.ApiGateway.Classroom.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
[Authorize]
public class ClassroomController : ControllerBase
{
    private readonly IClassroomRepository _classroomRepository;

    public ClassroomController(IClassroomRepository classroomRepository)
    {
        _classroomRepository = classroomRepository;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;

        var classrooms = await _classroomRepository.GetAsync(userId);

        return Ok(JsonConvert.SerializeObject(classrooms));
    }

    [HttpPost]
    [Authorize(Roles = Role.UniversityAdmin)]
    public async Task<IActionResult> Add([FromBody] AddClassroomDTO addClassroomDTO)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;

        var addAsyncResult = await _classroomRepository.AddAsync(addClassroomDTO, userId);

        if (addAsyncResult == null) return NotFound(JsonConvert.SerializeObject(Message.UserIdNotFound));

        if (addAsyncResult.Value > 0)
            return Ok(JsonConvert.SerializeObject(new ClassroomDTO(addClassroomDTO) {Id = addAsyncResult.Value}));

        return BadRequest();
    }

    [HttpPost]
    [Authorize(Roles = Role.UniversityAdmin)]
    public async Task<IActionResult> AddUser([FromQuery] int classroomId, [FromQuery] string userId)
    {
        var requestUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;

        var addUserAsyncResult = await _classroomRepository.AddUserAsync(classroomId, requestUserId, userId);

        if (addUserAsyncResult == null) return NotFound(JsonConvert.SerializeObject(Message.ClassroomNotFound));

        if (addUserAsyncResult.Value) return Ok(JsonConvert.SerializeObject(Message.AddClassroomUser(userId, classroomId)));

        return BadRequest();
    }

    [HttpPut]
    [Authorize(Roles = Role.UniversityAdmin)]
    public async Task<IActionResult> Update([FromBody] ClassroomDTO classroomDTO)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;

        var updateAsyncResult = await _classroomRepository.UpdateAsync(classroomDTO, userId);

        if (updateAsyncResult == null) return NotFound(JsonConvert.SerializeObject(Message.ClassroomNotFound));

        if (updateAsyncResult.Value) return Ok(JsonConvert.SerializeObject(classroomDTO));

        return BadRequest();
    }

    [HttpDelete]
    [Authorize(Roles = Role.UniversityAdmin)]
    public async Task<IActionResult> RemoveUser([FromQuery] int classroomId, [FromQuery] string userId)
    {
        var requestUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;

        var removeUserAsyncResult = await _classroomRepository.RemoveUserAsync(classroomId, requestUserId, userId);

        if (removeUserAsyncResult == null) return NotFound(JsonConvert.SerializeObject(Message.ClassroomNotFound));

        if (removeUserAsyncResult.Value)
            return Ok(JsonConvert.SerializeObject(Message.RemoveClassroomUser(userId, classroomId)));

        return NotFound(JsonConvert.SerializeObject(Message.ClassroomNotFound));
    }

    [HttpDelete]
    [Authorize(Roles = Role.UniversityAdmin)]
    public async Task<IActionResult> Remove([FromQuery] int classroomId)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;

        var removeAsyncResult = await _classroomRepository.RemoveAsync(classroomId, userId);

        if (removeAsyncResult == null) return NotFound(JsonConvert.SerializeObject(Message.ClassroomNotFound));

        if (removeAsyncResult.Value) return Ok(JsonConvert.SerializeObject(Message.ClassroomRemoved));

        return BadRequest();
    }
}