using LearningLantern.ApiGateway.Data.DTOs;
using LearningLantern.ApiGateway.Helpers;
using LearningLantern.ApiGateway.Repositories;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace LearningLantern.ApiGateway.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IIdentityRepository _identityRepository;

    public AuthController(IIdentityRepository identityRepository)
    {
        _identityRepository = identityRepository;
    }

    [HttpPost]
    public async Task<IActionResult> SignUp([FromBody] SignUpDTO signUpDTO)
    {
        if (!Helper.IsUniversityValid(signUpDTO.University))
            return BadRequest(JsonConvert.SerializeObject(Message.UniversityNotFound));

        if (!Helper.IsNameValid(signUpDTO.FirstName) || !Helper.IsNameValid(signUpDTO.LastName))
            return BadRequest(JsonConvert.SerializeObject(Message.NameNotValid));

        var createAsyncResult = await _identityRepository.SignUpAsync(signUpDTO);

        return createAsyncResult.Succeeded
            ? CreatedAtAction(nameof(ConfirmEmail),
                JsonConvert.SerializeObject(signUpDTO.Email))
            : BadRequest(JsonConvert.SerializeObject(createAsyncResult.Errors));
    }

    [HttpPost]
    public async Task<IActionResult> SignIn([FromBody] SignInDTO signInDTO)
    {
        if (!Helper.IsUniversityValid(signInDTO.University))
            return BadRequest(JsonConvert.SerializeObject(Message.UniversityNotFound));

        var signInResponseDTO = await _identityRepository.SignInAsync(signInDTO);

        if (signInResponseDTO.User == null) return NotFound(JsonConvert.SerializeObject(Message.UserEmailNotFound));

        return string.IsNullOrEmpty(signInResponseDTO.Token)
            ? Unauthorized(JsonConvert.SerializeObject("Can not get JWT."))
            : Ok(JsonConvert.SerializeObject(signInResponseDTO));
    }

    [HttpGet]
    public async Task<IActionResult> ResendConfirmationEmail([FromQuery] string userEmail)
    {
        var sendConfirmationEmailResult = await _identityRepository.SendConfirmationEmailAsync(userEmail);

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
        var confirmEmailAsyncResult = await _identityRepository.ConfirmEmailAsync(userId, token);

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