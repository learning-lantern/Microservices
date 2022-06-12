using LearningLantern.ApiGateway.Data.DTOs;
using LearningLantern.ApiGateway.Repositories;
using LearningLantern.Common;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace LearningLantern.ApiGateway.Controllers;

[Route("api/[controller]/[action]")]
public class UserController : ApiControllerBase
{
    private readonly IIdentityRepository _identityRepository;
    private readonly ILogger<UserController> _logger;

    public UserController(IIdentityRepository identityRepository, ILogger<UserController> logger)
    {
        _identityRepository = identityRepository;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> SignUp([FromBody] SignUpDTO signUpDTO)
    {
        var signUpResult = await _identityRepository.SignUpAsync(signUpDTO);

        if (signUpResult.Succeeded)
            signUpResult =
                await _identityRepository.SendConfirmationEmailAsync(signUpDTO.Email, Url.ActionLink(nameof(ConfirmEmail)));

        if (signUpResult.Succeeded)
            return CreatedAtAction(nameof(ConfirmEmail), JsonConvert.SerializeObject(signUpDTO.Email));

        return ResponseToIActionResult(signUpResult);
    }

    [HttpPost]
    public async Task<IActionResult> SignIn([FromBody] SignInDTO signInDTO)
    {
        var signInResponse = await _identityRepository.SignInAsync(signInDTO);

        return ResponseToIActionResult(signInResponse);
    }

    [HttpGet]
    public async Task<IActionResult> ResendConfirmationEmail([FromQuery] string userEmail)
    {
        var response =
            await _identityRepository.SendConfirmationEmailAsync(userEmail, Url.ActionLink(nameof(ConfirmEmail)));

        return ResponseToIActionResult(response);
    }

    [HttpGet]
    public async Task<IActionResult> ConfirmEmail([FromQuery] string userId, [FromQuery] string token)
    {
        var response = await _identityRepository.ConfirmEmailAsync(userId, token);

        return ResponseToIActionResult(response);
    }
}