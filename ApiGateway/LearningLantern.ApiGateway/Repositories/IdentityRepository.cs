using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Web;
using LearningLantern.ApiGateway.Data.DTOs;
using LearningLantern.ApiGateway.Data.Models;
using LearningLantern.ApiGateway.Services;
using LearningLantern.ApiGateway.Utility;
using LearningLantern.Common.DependencyInjection;
using LearningLantern.Common.Response;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace LearningLantern.ApiGateway.Repositories;

public class IdentityRepository : IIdentityRepository
{
    private readonly IEmailSender _emailSender;
    private readonly SignInManager<UserModel> _signInManager;
    private readonly UserManager<UserModel> _userManager;

    public IdentityRepository(
        UserManager<UserModel> userManager, SignInManager<UserModel> signInManager, IEmailSender emailSender)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _emailSender = emailSender;
    }

    public async Task<Response> SignUpAsync(SignUpDTO signUpDTO)
    {
        var result = Validator.ValidateSignUpDTO(signUpDTO);
        if (result.Count > 0) return ResponseFactory.Fail(result);

        var user = new UserModel
        {
            UserName = signUpDTO.Email,
            Email = signUpDTO.Email,
            FirstName = signUpDTO.FirstName.Trim(),
            LastName = signUpDTO.LastName.Trim()
        };

        var createAsyncResult = await _userManager.CreateAsync(user, signUpDTO.Password);

        if (createAsyncResult.Succeeded) return await SendConfirmationEmailAsync(user.Email);

        return createAsyncResult.ToApplicationResponse();
    }

    public async Task<Response> SendConfirmationEmailAsync(string userEmail)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(userEmail);

            if (user == null) return ResponseFactory.Fail(ErrorsList.UserEmailNotFound());

            var token = HttpUtility.UrlEncode(await _userManager.GenerateEmailConfirmationTokenAsync(user));

            var mailMessage = MessageTemplates.ConfirmationEmail(user.Id, token);

            await _emailSender.Send(userEmail, mailMessage);

            return ResponseFactory.Ok();
        }
        catch (Exception ex)
        {
            return ResponseFactory.Fail(new Error
            {
                ErrorCode = "ConfirmationFailure",
                Description = ex.Message
            });
        }
    }

    public async Task<Response<SignInResponseDTO>> SignInAsync(SignInDTO signInDTO)
    {
        var validateResult = Validator.ValidateSignInDTO(signInDTO);
        if (validateResult.Count > 0) return ResponseFactory.Fail<SignInResponseDTO>(validateResult, null);

        if ((await FindByEmailAsync(signInDTO.Email)).Succeeded == false)
            return ResponseFactory.Fail<SignInResponseDTO>(ErrorsList.UserEmailNotFound(), null);

        var passwordSignInAsyncResult = await _signInManager.PasswordSignInAsync(
            signInDTO.Email, signInDTO.Password,
            true, false);

        if (passwordSignInAsyncResult.IsNotAllowed)
            return ResponseFactory.Fail<SignInResponseDTO>(ErrorsList.SignInNotAllowed(), null);
        if (passwordSignInAsyncResult.Succeeded == false)
            return ResponseFactory.Fail<SignInResponseDTO>(ErrorsList.SignInFailed(), null);


        var user = await _userManager.FindByEmailAsync(signInDTO.Email);

        var token = await GenerateTokens(user);

        var result = new SignInResponseDTO(new UserDTO(user), token);

        return ResponseFactory.Ok(result);
    }

    public async Task<Response> ConfirmEmailAsync(string userId, string token)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user is null) return ResponseFactory.Fail(ErrorsList.UserIdNotFound());

        var result = await _userManager.ConfirmEmailAsync(user, token);

        return result.ToApplicationResponse();
    }

    public async Task<UserModel?> FindUserByIdAsync(string userId) => await _userManager.FindByIdAsync(userId);

    async private Task<Response<UserDTO>> FindByEmailAsync(string userEmail)
    {
        var user = await _userManager.FindByEmailAsync(userEmail);

        if (user is null || user.EmailConfirmed == false)
            return ResponseFactory.Fail<UserDTO>(ErrorsList.UserEmailNotFound(), default);
        return ResponseFactory.Ok(new UserDTO(user));
    }

    async private Task<string> GenerateTokens(UserModel user)
    {
        var roles = await _userManager.GetRolesAsync(user);

        var claims = new List<Claim>
        {
            new(ClaimTypes.Email, user.Email),
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var token = new JwtSecurityToken(
            JWT.ValidIssuer,
            JWT.ValidAudience,
            claims,
            expires: DateTime.UtcNow.AddDays(30),
            signingCredentials: new SigningCredentials(JWT.IssuerSigningKey,
                SecurityAlgorithms.HmacSha256Signature)
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}