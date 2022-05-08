using System.Security.Claims;
using LearningLantern.ApiGateway.Data.DTOs;
using LearningLantern.ApiGateway.Helpers;
using LearningLantern.ApiGateway.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace LearningLantern.ApiGateway.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
[Authorize]
public class UserController : ControllerBase
{
    private readonly IIdentityRepository _identityRepository;

    public UserController(IIdentityRepository identityRepository)
    {
        _identityRepository = identityRepository;
    }

    [HttpGet]
    public async Task<IActionResult> FindById([FromQuery] string userId)
    {
        var user = await _identityRepository.FindByIdAsync(userId);

        return user == null
            ? NotFound(JsonConvert.SerializeObject(Message.UserIdNotFound))
            : Ok(JsonConvert.SerializeObject(user));
    }

    [HttpGet]
    public async Task<IActionResult> FindByEmail([FromQuery] string userEmail)
    {
        var user = await _identityRepository.FindByEmailAsync(userEmail);

        return user == null
            ? NotFound(JsonConvert.SerializeObject(Message.UserEmailNotFound))
            : Ok(JsonConvert.SerializeObject(user));
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UserDTO userDTO)
    {
        if (!Helper.IsUniversityValid(userDTO.University))
            return BadRequest(JsonConvert.SerializeObject(Message.UniversityNotFound));

        if (!Helper.IsNameValid(userDTO.FirstName) || !Helper.IsNameValid(userDTO.LastName))
            return BadRequest(JsonConvert.SerializeObject(Message.NameNotValid));

        userDTO.Id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;

        var user = await _identityRepository.UpdateAsync(userDTO);

        return user == null
            ? NotFound(JsonConvert.SerializeObject(Message.UserIdNotFound))
            : Ok(JsonConvert.SerializeObject(user));
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromBody] SignInDTO signInDTO)
    {
        if (!Helper.IsUniversityValid(signInDTO.University))
            return BadRequest(JsonConvert.SerializeObject(Message.UniversityNotFound));

        var deleteAsyncResult = await _identityRepository.DeleteAsync(signInDTO.Email, signInDTO.Password);

        if (!deleteAsyncResult.Succeeded)
        {
            if (deleteAsyncResult.Errors?.FirstOrDefault(error => error.Code == StatusCodes.Status404NotFound.ToString()) !=
                null) return NotFound(JsonConvert.SerializeObject(deleteAsyncResult.Errors));

            return BadRequest(JsonConvert.SerializeObject(deleteAsyncResult.Errors));
        }

        return Ok(JsonConvert.SerializeObject(Message.UserDeleted));
    }
}