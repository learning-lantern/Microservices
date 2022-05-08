using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Security.Claims;
using System.Web;
using LearningLantern.ApiGateway.Configurations;
using LearningLantern.ApiGateway.Data.DTOs;
using LearningLantern.ApiGateway.Data.Models;
using LearningLantern.ApiGateway.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace LearningLantern.ApiGateway.Repositories;

public class IdentityRepository : IIdentityRepository
{
    private readonly SignInManager<UserModel> _signInManager;
    private readonly UserManager<UserModel> _userManager;

    public IdentityRepository(UserManager<UserModel> userManager, SignInManager<UserModel> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<IdentityResult> SignUpAsync(SignUpDTO signUpDTO)
    {
        var user = new UserModel
        {
            UserName = signUpDTO.Email,
            Email = signUpDTO.Email,
            FirstName = signUpDTO.FirstName.Trim(),
            LastName = signUpDTO.LastName.Trim()
        };

        var createAsyncResult = await _userManager.CreateAsync(user, signUpDTO.Password);

        return createAsyncResult.Succeeded ? await SendConfirmationEmailAsync(user.Email) : createAsyncResult;
    }

    public async Task<SignInResponseDTO> SignInAsync(SignInDTO signInDTO)
    {
        var passwordSignInAsyncResult = await _signInManager.PasswordSignInAsync(
            signInDTO.Email, signInDTO.Password,
            true, false);

        if (!passwordSignInAsyncResult.Succeeded) return new SignInResponseDTO();

        var user = await _userManager.FindByEmailAsync(signInDTO.Email);
        var roles = await _userManager.GetRolesAsync(user);

        var claims = new List<Claim>
        {
            new(ClaimTypes.Email, signInDTO.Email),
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        foreach (var role in roles) claims.Add(new Claim(ClaimTypes.Role, role));

        var token = new JwtSecurityToken(
            ConfigProvider.JWTValidIssuer,
            ConfigProvider.JWTValidAudience,
            claims,
            expires: DateTime.UtcNow.AddDays(30),
            signingCredentials: new SigningCredentials(ConfigProvider.JWTIssuerSigningKey,
                SecurityAlgorithms.HmacSha256Signature)
        );

        return new SignInResponseDTO(new UserDTO(user), new JwtSecurityTokenHandler().WriteToken(token));
    }

    public async Task<IdentityResult> SendConfirmationEmailAsync(string userEmail)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(userEmail);

            if (user == null)
                return IdentityResult.Failed(new IdentityError
                {
                    Code = StatusCodes.Status404NotFound.ToString(),
                    Description = Message.UserEmailNotFound
                });

            var token = HttpUtility.UrlEncode(await _userManager.GenerateEmailConfirmationTokenAsync(user));

            var mailMessage = new MailMessage(Message.FromEmail, userEmail, Message.EmailSubject,
                Message.EmailBody(user.Id, token))
            {
                IsBodyHtml = true
            };

            await Message.Client.SendMailAsync(mailMessage);

            return IdentityResult.Success;
        }
        catch (Exception ex)
        {
            return IdentityResult.Failed(new IdentityError
            {
                Code = "ConfirmationFailure",
                Description = ex.Message
            });
        }
    }

    public async Task<IdentityResult> ConfirmEmailAsync(string userId, string token)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
            return IdentityResult.Failed(new IdentityError
            {
                Code = StatusCodes.Status404NotFound.ToString(),
                Description = Message.UserIdNotFound
            });

        return await _userManager.ConfirmEmailAsync(user, token);
    }

    public async Task<UserDTO?> FindByIdAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        return user == null || !user.EmailConfirmed ? null : new UserDTO(user);
    }

    public async Task<UserDTO?> FindByEmailAsync(string userEmail)
    {
        var user = await _userManager.FindByEmailAsync(userEmail);

        return user == null || !user.EmailConfirmed ? null : new UserDTO(user);
    }

    public async Task<UserDTO?> UpdateAsync(UserDTO userDTO)
    {
        var user = await _userManager.FindByIdAsync(userDTO.Id);

        if (user == null) return null;

        user.FirstName = userDTO.FirstName.Trim();
        user.LastName = userDTO.LastName.Trim();

        var updateAsyncResult = await _userManager.UpdateAsync(user);

        return updateAsyncResult.Succeeded ? new UserDTO(user) : null;
    }

    public async Task<IdentityResult> DeleteAsync(string userEmail, string userPassword)
    {
        var user = await _userManager.FindByEmailAsync(userEmail);

        if (user == null)
            return IdentityResult.Failed(new IdentityError
            {
                Code = StatusCodes.Status404NotFound.ToString(),
                Description = Message.UserEmailNotFound
            });

        return await _userManager.DeleteAsync(user);
    }

    public async Task<UserModel?> FindUserByIdAsync(string userId) => await _userManager.FindByIdAsync(userId);
}