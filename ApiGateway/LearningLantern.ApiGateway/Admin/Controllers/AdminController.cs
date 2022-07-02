using LearningLantern.ApiGateway.Admin.Repositories;
using LearningLantern.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace LearningLantern.ApiGateway.Admin.Controllers;

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

    [HttpGet]
    public async Task<IActionResult> CreateAdminRole()
    {
        var createAdminRoleAsyncResult = await _adminRepository.CreateAdminRoleAsync();

        return createAdminRoleAsyncResult.Succeeded
            ? Ok(JsonConvert.SerializeObject(AdminMessages.CreatedRole(LearningLanternRoles.Admin)))
            : BadRequest(JsonConvert.SerializeObject(createAdminRoleAsyncResult.Errors));
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

    [HttpGet]
    public async Task<IActionResult> CreateUniversityAdminRole()
    {
        var createUniversityAdminRoleAsyncResult = await _adminRepository.CreateUniversityAdminRoleAsync();

        return createUniversityAdminRoleAsyncResult.Succeeded
            ? Ok(JsonConvert.SerializeObject(AdminMessages.CreatedRole(LearningLanternRoles.UniversityAdmin)))
            : BadRequest(JsonConvert.SerializeObject(createUniversityAdminRoleAsyncResult.Errors));
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
    public async Task<IActionResult> CreateInstructorRole()
    {
        var createInstructorRoleAsyncResult = await _adminRepository.CreateInstructorRoleAsync();

        return createInstructorRoleAsyncResult.Succeeded
            ? Ok(JsonConvert.SerializeObject(AdminMessages.CreatedRole(LearningLanternRoles.Instructor)))
            : BadRequest(JsonConvert.SerializeObject(createInstructorRoleAsyncResult.Errors));
    }
}