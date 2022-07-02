using FluentValidation;
using LearningLantern.ApiGateway.Data.DTOs;
using LearningLantern.ApiGateway.Data.Models;
using LearningLantern.ApiGateway.Users.BuildingBlocks;
using LearningLantern.ApiGateway.Utility;
using LearningLantern.Common.Response;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace LearningLantern.ApiGateway.Users.Commands;

public class LoginDTO
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}

public class LoginCommand : IHaveEmail, IHavePassword, IRequest<Response<TokenResponseDTO>>
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

public class LoginCommandCommandHandler : IRequestHandler<LoginCommand, Response<TokenResponseDTO>>
{
    private readonly JWTGenerator _jwtGenerator;
    private readonly SignInManager<UserModel> _signInManager;
    private readonly UserManager<UserModel> _userManager;

    public LoginCommandCommandHandler(
        SignInManager<UserModel> signInManager, UserManager<UserModel> userManager, JWTGenerator jwtGenerator)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _jwtGenerator = jwtGenerator;
    }

    public async Task<Response<TokenResponseDTO>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null)
            return ResponseFactory.Fail<TokenResponseDTO>(ErrorsList.UserEmailNotFound(request.Email));

        var passwordSignInAsyncResult = await _signInManager.PasswordSignInAsync(
            request.Email, request.Password,
            true, false);

        if (passwordSignInAsyncResult.IsNotAllowed)
            return ResponseFactory.Fail<TokenResponseDTO>(ErrorsList.SignInNotAllowed());
        if (passwordSignInAsyncResult.Succeeded == false)
            return ResponseFactory.Fail<TokenResponseDTO>(ErrorsList.SignInFailed());

        var result = await _jwtGenerator.GenerateJwtSecurityToken(user);

        return ResponseFactory.Ok(result);
    }
}