using LearningLantern.ApiGateway.Auth.DTOs;
using LearningLantern.ApiGateway.Auth.Repositories;
using LearningLantern.ApiGateway.Helpers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace LearningLantern.ApiGateway.Auth.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthRepository _authRepository;

    public AuthController(IAuthRepository authRepository)
    {
        _authRepository = authRepository;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateDTO createDTO)
    {
        if (!Helper.IsUniversityValid(createDTO.University))
            return BadRequest(JsonConvert.SerializeObject(Message.UniversityNotFound));

        if (!Helper.IsNameValid(createDTO.FirstName) || !Helper.IsNameValid(createDTO.LastName))
            return BadRequest(JsonConvert.SerializeObject(Message.NameNotValid));

        var createAsyncResult = await _authRepository.CreateAsync(createDTO);

        return createAsyncResult.Succeeded
            ? CreatedAtAction(nameof(ConfirmEmail),
                JsonConvert.SerializeObject(createDTO.Email))
            : BadRequest(JsonConvert.SerializeObject(createAsyncResult.Errors));
    }

    [HttpPost]
    public async Task<IActionResult> SignIn([FromBody] SignInDTO signInDTO)
    {
        if (!Helper.IsUniversityValid(signInDTO.University))
            return BadRequest(JsonConvert.SerializeObject(Message.UniversityNotFound));

        var signInResponseDTO = await _authRepository.SignInAsync(signInDTO);

        if (signInResponseDTO.User == null) return NotFound(JsonConvert.SerializeObject(Message.UserEmailNotFound));

        return string.IsNullOrEmpty(signInResponseDTO.Token)
            ? Unauthorized(JsonConvert.SerializeObject("Can not get JWT."))
            : Ok(JsonConvert.SerializeObject(signInResponseDTO));
    }

    [HttpGet]
    public async Task<IActionResult> ResendConfirmationEmail([FromQuery] string userEmail)
    {
        var sendConfirmationEmailResult = await _authRepository.SendConfirmationEmailAsync(userEmail);

        if (!sendConfirmationEmailResult.Succeeded)
        {
            if (sendConfirmationEmailResult.Errors?.FirstOrDefault(error =>
                    error.Code == StatusCodes.Status404NotFound.ToString()) !=
                null) return NotFound(JsonConvert.SerializeObject(sendConfirmationEmailResult.Errors));

            return BadRequest(JsonConvert.SerializeObject(sendConfirmationEmailResult.Errors));
        }

        return Ok(JsonConvert.SerializeObject(userEmail));
    }

    [HttpGet]
    public async Task<IActionResult> ConfirmEmail([FromQuery] string userId, [FromQuery] string token)
    {
        var confirmEmailAsyncResult = await _authRepository.ConfirmEmailAsync(userId, token);

        if (!confirmEmailAsyncResult.Succeeded)
        {
            if (confirmEmailAsyncResult.Errors?.FirstOrDefault(error =>
                    error.Code == StatusCodes.Status404NotFound.ToString()) !=
                null) return NotFound(JsonConvert.SerializeObject(confirmEmailAsyncResult.Errors));

            return BadRequest(JsonConvert.SerializeObject(confirmEmailAsyncResult.Errors));
        }

        return Ok(JsonConvert.SerializeObject(userId));
    }
}