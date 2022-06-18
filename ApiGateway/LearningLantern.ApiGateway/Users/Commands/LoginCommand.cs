using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AutoMapper;
using FluentValidation;
using LearningLantern.ApiGateway.Users.BuildingBlocks;
using LearningLantern.ApiGateway.Users.Models;
using LearningLantern.ApiGateway.Utility;
using LearningLantern.Common.DependencyInjection;
using LearningLantern.Common.Response;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace LearningLantern.ApiGateway.Users.Commands;

public class LoginDTO
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}

public class LoginCommand : IHaveEmail, IHavePassword, IRequest<Response<SignInResponseDTO>>
{
    public LoginCommand(LoginDTO loginDTO)
    {
        Email = loginDTO.Email;
        Password = loginDTO.Password;
    }

    public string Email { get; set; }
    public string Password { get; set; }
}

public class LoginDTOCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginDTOCommandValidator()
    {
        Include(new EmailValidator());
        Include(new PasswordValidator());
    }
}

public class LoginCommandCommandHandler : IRequestHandler<LoginCommand, Response<SignInResponseDTO>>
{
    private readonly IMapper _mapper;
    private readonly SignInManager<UserModel> _signInManager;
    private readonly UserManager<UserModel> _userManager;

    public LoginCommandCommandHandler(
        IMapper mapper, SignInManager<UserModel> signInManager, UserManager<UserModel> userManager)
    {
        _mapper = mapper;
        _signInManager = signInManager;
        _userManager = userManager;
    }

    public async Task<Response<SignInResponseDTO>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null)
            return ResponseFactory.Fail<SignInResponseDTO>(ErrorsList.UserEmailNotFound(request.Email));

        var passwordSignInAsyncResult = await _signInManager.PasswordSignInAsync(
            request.Email, request.Password,
            true, false);

        if (passwordSignInAsyncResult.IsNotAllowed)
            return ResponseFactory.Fail<SignInResponseDTO>(ErrorsList.SignInNotAllowed());
        if (passwordSignInAsyncResult.Succeeded == false)
            return ResponseFactory.Fail<SignInResponseDTO>(ErrorsList.SignInFailed());

        var result = new SignInResponseDTO(_mapper.Map<UserDTO>(user), await GenerateJwtSecurityToken(user));

        return ResponseFactory.Ok(result);
    }

    async private Task<string> GenerateJwtSecurityToken(UserModel user)
    {
        var roles = await _userManager.GetRolesAsync(user);

        var claims = new List<Claim>
        {
            new(ClaimTypes.Email, user.Email),
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        return new JwtSecurityTokenHandler().WriteToken(new JwtSecurityToken(
            JWT.ValidIssuer,
            JWT.ValidAudience,
            claims,
            expires: DateTime.UtcNow.AddDays(30),
            signingCredentials: new SigningCredentials(JWT.IssuerSigningKey,
                SecurityAlgorithms.HmacSha256Signature)
        ));
    }
}