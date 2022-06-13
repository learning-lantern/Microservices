using System.ComponentModel.DataAnnotations;
using LearningLantern.ApiGateway.User.DTOs;
using LearningLantern.ApiGateway.User.Repository;
using LearningLantern.Common;
using LearningLantern.Common.Response;
using LearningLantern.Common.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearningLantern.ApiGateway.User.Controllers;

[Authorize]
[Route("api/user")]
public class UserController : ApiControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly ICurrentUserService _userService;

    public UserController(IUserRepository userRepository, ICurrentUserService userService)
    {
        _userRepository = userRepository;
        _userService = userService;
    }

    [AllowAnonymous]
    [HttpPost("signup")]
    [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
    public async Task<IActionResult> Signup([FromBody] SignupDTO signupDTO)
    {
        var signupResult = await _userRepository.Signup(signupDTO);

        if (signupResult.Succeeded)
            signupResult =
                await _userRepository.SendConfirmationEmail(signupDTO.Email, Url.ActionLink(nameof(ConfirmEmail)));

        return ResponseToIActionResult(signupResult);
    }

    [AllowAnonymous]
    [HttpPost("login")]
    [ProducesResponseType(typeof(Response<SignInResponseDTO>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
    {
        var signInResponse = await _userRepository.Login(loginDTO);
        return ResponseToIActionResult(signInResponse);
    }

    [AllowAnonymous]
    [HttpGet("validate-email")]
    [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
    public async Task<IActionResult> ConfirmEmail([FromQuery] string userId, [FromQuery] string token)
    {
        var response = await _userRepository.ConfirmEmail(userId, token);

        return ResponseToIActionResult(response);
    }

    [AllowAnonymous]
    [HttpGet("confirm-change-email")]
    [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
    public async Task<IActionResult> ConfirmUpdateEmail(
        [FromQuery] [Required] string userId, [FromQuery] [Required] string newEmail, [FromQuery] [Required] string token)
    {
        var response = await _userRepository.ConfirmUpdateEmail(userId, newEmail, token);
        return ResponseToIActionResult(response);
    }

    [HttpGet("resend-validation")]
    [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
    public async Task<IActionResult> ResendConfirmationEmail([FromQuery] string userEmail)
    {
        var response =
            await _userRepository.SendConfirmationEmail(userEmail, Url.ActionLink(nameof(ConfirmEmail)));

        return ResponseToIActionResult(response);
    }

    [HttpGet("all")]
    [ProducesResponseType(typeof(Response<IEnumerable<UserDTO>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllUsers([FromQuery] [Required] int page, [FromQuery] [Required] int limit)
    {
        var response = await _userRepository.GetAllUsers(page, limit);
        return ResponseToIActionResult(response);
    }

    [HttpGet("one/{userId}")]
    [ProducesResponseType(typeof(Response<UserDTO>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUser([FromRoute] string userId)
    {
        var response = await _userRepository.GetById(userId);
        return ResponseToIActionResult(response);
    }

    [HttpPut("update")]
    [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateName([FromBody] UpdateNameDTO updateNameDTO)
    {
        var userId = _userService.UserId;
        var response = await _userRepository.UpdateName(userId!, updateNameDTO);
        return ResponseToIActionResult(response);
    }

    [HttpPut("change-password")]
    [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordDTO updatePasswordDTO)
    {
        var userId = _userService.UserId;
        var response = await _userRepository.UpdatePassword(userId!, updatePasswordDTO);
        return ResponseToIActionResult(response);
    }

    [HttpPut("change-email")]
    [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateEmail([FromBody] UpdateEmailDTO updateEmailDTO)
    {
        var userId = _userService.UserId;
        var response =
            await _userRepository.UpdateEmail(userId!, updateEmailDTO, Url.ActionLink(nameof(ConfirmUpdateEmail)));
        return ResponseToIActionResult(response);
    }

    [HttpDelete]
    [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteUser([FromBody] DeleteUserDTO deleteUserDTO)
    {
        var userId = _userService.UserId;
        var response = await _userRepository.DeleteUser(userId!, deleteUserDTO);
        return ResponseToIActionResult(response);
    }
}