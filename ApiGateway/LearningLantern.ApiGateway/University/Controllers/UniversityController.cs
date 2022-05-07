using LearningLantern.ApiGateway.Helpers;
using LearningLantern.ApiGateway.University.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace LearningLantern.ApiGateway.University.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
[Authorize(Roles = Role.UniversityAdmin)]
public class UniversityController : ControllerBase
{
    private readonly IUniversityRepository universityAdminRepository;

    public UniversityController(IUniversityRepository universityAdminRepository)
    {
        this.universityAdminRepository = universityAdminRepository;
    }

    [HttpPost]
    public async Task<IActionResult> AddToRoleInstructor([FromQuery] string university, [FromQuery] string userId)
    {
        if (!Helper.IsUniversityValid(university))
            return BadRequest(JsonConvert.SerializeObject(Message.UniversityNotFound));

        var addToRoleInstructorAsyncResult = await universityAdminRepository.AddToRoleInstructorAsync(userId);

        if (!addToRoleInstructorAsyncResult.Succeeded)
        {
            if (addToRoleInstructorAsyncResult.Errors?.FirstOrDefault(error =>
                    error.Code == StatusCodes.Status404NotFound.ToString()) !=
                null) return NotFound(JsonConvert.SerializeObject(addToRoleInstructorAsyncResult.Errors));

            return BadRequest(JsonConvert.SerializeObject(addToRoleInstructorAsyncResult.Errors));
        }

        return Ok(JsonConvert.SerializeObject(Message.UpdateUserRole(userId, Role.Instructor)));
    }
}