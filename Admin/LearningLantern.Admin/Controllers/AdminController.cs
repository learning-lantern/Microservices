using LearningLantern.Admin.Repositories;
using LearningLantern.Admin.Utility;
using LearningLantern.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace LearningLantern.Admin.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
[Authorize(Roles = LearningLanternRoles.Admin)]
public class AdminController : ControllerBase
{
    private readonly IAdminRepository _adminRepository;

    public AdminController(IAdminRepository adminRepository)
    {
        _adminRepository = adminRepository;
    }
    
    [HttpPost]
    public async Task<IActionResult> AddToRoleInstructor([FromQuery] string userId)
    {
        var addToRoleAdminAsyncResult = await _adminRepository.AddToRoleInstructorAsync(userId);

        if (addToRoleAdminAsyncResult.Succeeded)
            return Ok(JsonConvert.SerializeObject(AdminMessages.UpdateUserRole(userId, LearningLanternRoles.Instructor)));
        if (addToRoleAdminAsyncResult.Errors?.FirstOrDefault(error =>
                error.Code == StatusCodes.Status404NotFound.ToString()) !=
            null) return NotFound(JsonConvert.SerializeObject(addToRoleAdminAsyncResult.Errors));

        return BadRequest(JsonConvert.SerializeObject(addToRoleAdminAsyncResult.Errors));
    }

    [HttpPost]
    public async Task<IActionResult> AddToRoleAdmin([FromQuery] string userId)
    {
        var addToRoleAdminAsyncResult = await _adminRepository.AddToRoleAdminAsync(userId);

        if (addToRoleAdminAsyncResult.Succeeded)
            return Ok(JsonConvert.SerializeObject(AdminMessages.UpdateUserRole(userId, LearningLanternRoles.Admin)));
        if (addToRoleAdminAsyncResult.Errors?.FirstOrDefault(error =>
                error.Code == StatusCodes.Status404NotFound.ToString()) !=
            null) return NotFound(JsonConvert.SerializeObject(addToRoleAdminAsyncResult.Errors));

        return BadRequest(JsonConvert.SerializeObject(addToRoleAdminAsyncResult.Errors));
    }
    

    [HttpPost]
    public async Task<IActionResult> AddToRoleUniversityAdmin([FromQuery] string userId)
    {
        var addToRoleUniversityAdminAsyncResult = await _adminRepository.AddToRoleUniversityAdminAsync(userId);

        if (addToRoleUniversityAdminAsyncResult.Succeeded)
            return Ok(JsonConvert.SerializeObject(AdminMessages.UpdateUserRole(userId, LearningLanternRoles.UniversityAdmin)));
        if (addToRoleUniversityAdminAsyncResult.Errors?.FirstOrDefault(error =>
                error.Code == StatusCodes.Status404NotFound.ToString()) != null)
            return NotFound(JsonConvert.SerializeObject(addToRoleUniversityAdminAsyncResult.Errors));

        return BadRequest(JsonConvert.SerializeObject(addToRoleUniversityAdminAsyncResult.Errors));
    }

    [HttpGet]
    public async Task<IActionResult> CreateRoles()
    {
        var createInstructorRoleAsyncResult = await _adminRepository.CreateRolesAsync();

        return createInstructorRoleAsyncResult.Succeeded
            ? Ok(JsonConvert.SerializeObject(AdminMessages.CreatedRoles()))
            : BadRequest(JsonConvert.SerializeObject(createInstructorRoleAsyncResult.Errors));
    }
}