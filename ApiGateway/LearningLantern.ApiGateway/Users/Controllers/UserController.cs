using System.ComponentModel.DataAnnotations;
using LearningLantern.ApiGateway.Users.Commands;
using LearningLantern.ApiGateway.Users.Models;
using LearningLantern.ApiGateway.Users.Queries;
using LearningLantern.Common;
using LearningLantern.Common.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearningLantern.ApiGateway.Users.Controllers;

[Authorize]
[Route("api/user")]
public class UserController : ApiControllerBase
{
    private readonly IMediator mediator;

    public UserController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [AllowAnonymous]
    [HttpPost("signup")]
    [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
    public async Task<IActionResult> Signup([FromBody] SignupDTO signupDTO)
    {
        var signupResult = await mediator.Send(new SignupCommand(signupDTO));
        return ResponseToIActionResult(signupResult);
    }

    [AllowAnonymous]
    [HttpPost("login")]
    [ProducesResponseType(typeof(Response<SignInResponseDTO>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
    {
        var loginResponse = await mediator.Send(new LoginCommand(loginDTO));
        return ResponseToIActionResult(loginResponse);
    }

    [AllowAnonymous]
    [HttpGet("validate-email")]
    [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
    public async Task<IActionResult> ConfirmEmail([FromQuery] [Required] string userId, [FromQuery] [Required] string token)
    {
        var response = await mediator.Send(new ConfirmEmailCommand {UserId = userId, Token = token});
        return ResponseToIActionResult(response);
    }

    [AllowAnonymous]
    [HttpGet("confirm-change-email")]
    [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
    public async Task<IActionResult> ConfirmUpdateEmail(
        [FromQuery] [Required] string userId, [FromQuery] [Required] string newEmail, [FromQuery] [Required] string token)
    {
        var response = await mediator.Send(
            new ConfirmNewEmailCommand {UserId = userId, Email = newEmail, Token = token});
        return ResponseToIActionResult(response);
    }

    [HttpGet("resend-validation")]
    [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
    public async Task<IActionResult> ResendConfirmationEmail()
    {
        var response = await mediator.Send(new SendConfirmationEmailCommand());
        return ResponseToIActionResult(response);
    }

    [HttpGet("all")]
    [ProducesResponseType(typeof(Response<IEnumerable<UserDTO>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllUsers([FromQuery] [Required] int page, [FromQuery] [Required] int limit)
    {
        var response = await mediator.Send(new GetAllUsersQuery {PageNumber = page, PageSize = limit});
        return ResponseToIActionResult(response);
    }

    [HttpGet("one/{userId}")]
    [ProducesResponseType(typeof(Response<UserDTO>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUser([FromRoute] string userId)
    {
        var response = await mediator.Send(new GetUserByIdQuery {UserId = userId});
        return ResponseToIActionResult(response);
    }

    [HttpPut("update")]
    [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateName([FromBody] UpdateNameDTO updateNameDTO)
    {
        var response = await mediator.Send(new UpdateNameCommand(updateNameDTO));
        return ResponseToIActionResult(response);
    }

    [HttpPut("change-password")]
    [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordDTO updatePasswordDTO)
    {
        var response = await mediator.Send(new UpdatePasswordCommand(updatePasswordDTO));
        return ResponseToIActionResult(response);
    }

    [HttpPut("change-email")]
    [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateEmail([FromBody] UpdateEmailDTO updateEmailDTO)
    {
        var response = await mediator.Send(new UpdateEmailCommand(updateEmailDTO));
        return ResponseToIActionResult(response);
    }

    [HttpDelete]
    [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteUser([FromBody] DeleteUserDTO deleteUserDTO)
    {
        var response = await mediator.Send(new DeleteUserCommand(deleteUserDTO));
        return ResponseToIActionResult(response);
    }
}