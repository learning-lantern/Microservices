using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Security.Claims;
using System.Web;
using LearningLantern.ApiGateway.Auth.DTOs;
using LearningLantern.ApiGateway.Helpers;
using LearningLantern.ApiGateway.User.DTOs;
using LearningLantern.ApiGateway.User.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace LearningLantern.ApiGateway.Auth.Repositories;

public class AuthRepository : IAuthRepository
{
    private readonly SignInManager<UserModel> signInManager;
    private readonly UserManager<UserModel> userManager;

    public AuthRepository(UserManager<UserModel> userManager, SignInManager<UserModel> signInManager)
    {
        this.userManager = userManager;
        this.signInManager = signInManager;
    }

    public async Task<IdentityResult> CreateAsync(CreateDTO createDTO)
    {
        var user = new UserModel
        {
            UserName = createDTO.Email,
            Email = createDTO.Email,
            FirstName = createDTO.FirstName.Trim(),
            LastName = createDTO.LastName.Trim()
        };

        var createAsyncResult = await userManager.CreateAsync(user, createDTO.Password);

        return createAsyncResult.Succeeded ? await SendConfirmationEmailAsync(user.Email) : createAsyncResult;
    }

    public async Task<SignInResponseDTO> SignInAsync(SignInDTO signInDTO)
    {
        var passwordSignInAsyncResult = await signInManager.PasswordSignInAsync(
            signInDTO.Email, signInDTO.Password,
            true, false);

        if (!passwordSignInAsyncResult.Succeeded) return new SignInResponseDTO();

        var user = await userManager.FindByEmailAsync(signInDTO.Email);
        var roles = await userManager.GetRolesAsync(user);

        var claims = new List<Claim>
        {
            new(ClaimTypes.Email, signInDTO.Email),
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        foreach (var role in roles) claims.Add(new Claim(ClaimTypes.Role, role));

        var token = new JwtSecurityToken(
            JWT.ValidIssuer,
            JWT.ValidAudience,
            claims,
            expires: DateTime.UtcNow.AddDays(30),
            signingCredentials: new SigningCredentials(JWT.IssuerSigningKey, SecurityAlgorithms.HmacSha256Signature)
        );

        return new SignInResponseDTO(new UserDTO(user),
            new JwtSecurityTokenHandler().WriteToken(token));
    }

    public async Task<IdentityResult> SendConfirmationEmailAsync(string userEmail)
    {
        try
        {
            var user = await userManager.FindByEmailAsync(userEmail);

            if (user == null)
                return IdentityResult.Failed(new IdentityError
                {
                    Code = StatusCodes.Status404NotFound.ToString(),
                    Description = Message.UserEmailNotFound
                });

            var token = HttpUtility.UrlEncode(await userManager.GenerateEmailConfirmationTokenAsync(user));

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
        var user = await userManager.FindByIdAsync(userId);

        if (user == null)
            return IdentityResult.Failed(new IdentityError
            {
                Code = StatusCodes.Status404NotFound.ToString(),
                Description = Message.UserIdNotFound
            });

        return await userManager.ConfirmEmailAsync(user, token);
    }
}