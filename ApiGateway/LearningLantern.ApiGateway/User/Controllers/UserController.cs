using System.Security.Claims;
using LearningLantern.ApiGateway.Auth.DTOs;
using LearningLantern.ApiGateway.Helpers;
using LearningLantern.ApiGateway.User.DTOs;
using LearningLantern.ApiGateway.User.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace LearningLantern.ApiGateway.User.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
[Authorize]
public class UserController : ControllerBase
{
    private readonly IUserRepository userRepository;

    public UserController(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    [HttpGet]
    public async Task<IActionResult> FindById([FromQuery] string userId)
    {
        var user = await userRepository.FindByIdAsync(userId);

        return user == null
            ? NotFound(JsonConvert.SerializeObject(Message.UserIdNotFound))
            : Ok(JsonConvert.SerializeObject(user));
    }

    [HttpGet]
    public async Task<IActionResult> FindByEmail([FromQuery] string userEmail)
    {
        var user = await userRepository.FindByEmailAsync(userEmail);

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

        var user = await userRepository.UpdateAsync(userDTO);

        return user == null
            ? NotFound(JsonConvert.SerializeObject(Message.UserIdNotFound))
            : Ok(JsonConvert.SerializeObject(user));
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromBody] SignInDTO signInDTO)
    {
        if (!Helper.IsUniversityValid(signInDTO.University))
            return BadRequest(JsonConvert.SerializeObject(Message.UniversityNotFound));

        var deleteAsyncResult = await userRepository.DeleteAsync(signInDTO.Email, signInDTO.Password);

        if (!deleteAsyncResult.Succeeded)
        {
            if (deleteAsyncResult.Errors?.FirstOrDefault(error => error.Code == StatusCodes.Status404NotFound.ToString()) !=
                null) return NotFound(JsonConvert.SerializeObject(deleteAsyncResult.Errors));

            return BadRequest(JsonConvert.SerializeObject(deleteAsyncResult.Errors));
        }

        return Ok(JsonConvert.SerializeObject(Message.UserDeleted));
    }
}