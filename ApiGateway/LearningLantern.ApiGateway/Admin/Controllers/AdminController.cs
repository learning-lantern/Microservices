using LearningLantern.ApiGateway.Admin.Repositories;
using LearningLantern.ApiGateway.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace LearningLantern.ApiGateway.Admin.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
[Authorize(Roles = Role.Admin)]
public class AdminController : ControllerBase
{
    private readonly IAdminRepository adminRepository;

    public AdminController(IAdminRepository adminRepository)
    {
        this.adminRepository = adminRepository;
    }

    [HttpGet]
    public async Task<IActionResult> CreateAdminRole()
    {
        var createAdminRoleAsyncResult = await adminRepository.CreateAdminRoleAsync();

        return createAdminRoleAsyncResult.Succeeded
            ? Ok(JsonConvert.SerializeObject(Message.CreatedRole(Role.Admin)))
            : BadRequest(JsonConvert.SerializeObject(createAdminRoleAsyncResult.Errors));
    }

    [HttpPost]
    public async Task<IActionResult> AddToRoleAdmin([FromQuery] string userId, [FromQuery] string university)
    {
        if (!Helper.IsUniversityValid(university))
            return BadRequest(JsonConvert.SerializeObject(Message.UniversityNotFound));

        var addToRoleAdminAsyncResult = await adminRepository.AddToRoleAdminAsync(userId);

        if (!addToRoleAdminAsyncResult.Succeeded)
        {
            if (addToRoleAdminAsyncResult.Errors?.FirstOrDefault(error =>
                    error.Code == StatusCodes.Status404NotFound.ToString()) !=
                null) return NotFound(JsonConvert.SerializeObject(addToRoleAdminAsyncResult.Errors));

            return BadRequest(JsonConvert.SerializeObject(addToRoleAdminAsyncResult.Errors));
        }

        return Ok(JsonConvert.SerializeObject(Message.UpdateUserRole(userId, Role.Admin)));
    }

    [HttpGet]
    public async Task<IActionResult> CreateUniversityAdminRole()
    {
        var createUniversityAdminRoleAsyncResult = await adminRepository.CreateUniversityAdminRoleAsync();

        return createUniversityAdminRoleAsyncResult.Succeeded
            ? Ok(JsonConvert.SerializeObject(Message.CreatedRole(Role.UniversityAdmin)))
            : BadRequest(JsonConvert.SerializeObject(createUniversityAdminRoleAsyncResult.Errors));
    }

    [HttpPost]
    public async Task<IActionResult> AddToRoleUniversityAdmin([FromQuery] string university, [FromQuery] string userId)
    {
        if (!Helper.IsUniversityValid(university))
            return BadRequest(JsonConvert.SerializeObject(Message.UniversityNotFound));

        var addToRoleUniversityAdminAsyncResult = await adminRepository.AddToRoleUniversityAdminAsync(userId);

        if (!addToRoleUniversityAdminAsyncResult.Succeeded)
        {
            if (addToRoleUniversityAdminAsyncResult.Errors?.FirstOrDefault(error =>
                    error.Code == StatusCodes.Status404NotFound.ToString()) != null)
                return NotFound(JsonConvert.SerializeObject(addToRoleUniversityAdminAsyncResult.Errors));

            return BadRequest(JsonConvert.SerializeObject(addToRoleUniversityAdminAsyncResult.Errors));
        }

        return Ok(JsonConvert.SerializeObject(Message.UpdateUserRole(userId, Role.UniversityAdmin)));
    }

    [HttpGet]
    public async Task<IActionResult> CreateInstructorRole()
    {
        var createInstructorRoleAsyncResult = await adminRepository.CreateInstructorRoleAsync();

        return createInstructorRoleAsyncResult.Succeeded
            ? Ok(JsonConvert.SerializeObject(Message.CreatedRole(Role.Instructor)))
            : BadRequest(JsonConvert.SerializeObject(createInstructorRoleAsyncResult.Errors));
    }
}