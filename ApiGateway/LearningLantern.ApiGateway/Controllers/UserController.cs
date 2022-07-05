using System.ComponentModel.DataAnnotations;
using LearningLantern.ApiGateway.Data.DTOs;
using LearningLantern.ApiGateway.Users.Commands;
using LearningLantern.ApiGateway.Users.Queries;
using LearningLantern.Common;
using LearningLantern.Common.Extensions;
using LearningLantern.Common.Response;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearningLantern.ApiGateway.Controllers;

[Authorize]
[Route("api/user")]
public class UserController : ApiControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly IMediator _mediator;

    public UserController(IMediator mediator, ILogger<UserController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [AllowAnonymous]
    [HttpPost("signup")]
    [ProducesResponseType(typeof(TokenResponseDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IEnumerable<Error>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Signup([FromBody] SignupDTO signupDTO)
    {
        var signupResult = await _mediator.Send(new SignupCommand(signupDTO));
        if (signupResult.Succeeded) _mediator.Send(new SendConfirmationEmailCommand());
        return ResponseToIActionResult(signupResult);
    }

    [AllowAnonymous]
    [HttpPost("login")]
    [ProducesResponseType(typeof(TokenResponseDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IEnumerable<Error>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(IEnumerable<Error>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
    {
        var loginResponse = await _mediator.Send(new LoginCommand(loginDTO));
        return ResponseToIActionResult(loginResponse);
    }

    [AllowAnonymous]
    [HttpPost("validate-email")]
    [ProducesResponseType(typeof(TokenResponseDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IEnumerable<Error>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(IEnumerable<Error>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailCommand confirmEmailCommand)
    {
        var response = await _mediator.Send(confirmEmailCommand);
        _logger.LogInformation(response.ToJsonStringContent());
        return ResponseToIActionResult(response);
    }

    [AllowAnonymous]
    [HttpPost("confirm-change-email")]
    [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
    public async Task<IActionResult> ConfirmUpdateEmail([FromBody] ConfirmNewEmailCommand commandNewEmailCommand)
    {
        var response = await _mediator.Send(commandNewEmailCommand);
        return ResponseToIActionResult(response);
    }

    [HttpGet("resend-validation")]
    [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
    public async Task<IActionResult> ResendConfirmationEmail()
    {
        var response = await _mediator.Send(new SendConfirmationEmailCommand());
        return ResponseToIActionResult(response);
    }

    [Authorize(Roles = LearningLanternRoles.Admin + "," + LearningLanternRoles.UniversityAdmin)]
    [HttpGet("all")]
    [ProducesResponseType(typeof(Response<IEnumerable<UserDTO>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllUsers([FromQuery] [Required] int page, [FromQuery] [Required] int limit)
    {
        var response = await _mediator.Send(new GetAllUsersQuery {PageNumber = page, PageSize = limit});
        return ResponseToIActionResult(response);
    }

    [HttpGet("one/{userId}")]
    [ProducesResponseType(typeof(Response<UserDTO>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUser([FromRoute] string userId)
    {
        var response = await _mediator.Send(new GetUserByIdQuery {UserId = userId});
        return ResponseToIActionResult(response);
    }

    [HttpPut("update")]
    [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateName([FromBody] UpdateNameDTO updateNameDTO)
    {
        var response = await _mediator.Send(new UpdateNameCommand(updateNameDTO));
        return ResponseToIActionResult(response);
    }

    [HttpPut("change-password")]
    [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordDTO updatePasswordDTO)
    {
        var response = await _mediator.Send(new UpdatePasswordCommand(updatePasswordDTO));
        return ResponseToIActionResult(response);
    }

    [HttpPut("change-email")]
    [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateEmail([FromBody] UpdateEmailDTO updateEmailDTO)
    {
        var response = await _mediator.Send(new UpdateEmailCommand(updateEmailDTO));
        return ResponseToIActionResult(response);
    }

    [HttpDelete]
    [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteUser([FromBody] DeleteUserDTO deleteUserDTO)
    {
        var response = await _mediator.Send(new DeleteUserCommand(deleteUserDTO));
        return ResponseToIActionResult(response);
    }
}