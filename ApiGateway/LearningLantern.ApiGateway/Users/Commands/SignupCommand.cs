using AutoMapper;
using FluentValidation;
using LearningLantern.ApiGateway.Data.DTOs;
using LearningLantern.ApiGateway.Data.Models;
using LearningLantern.ApiGateway.Users.BuildingBlocks;
using LearningLantern.ApiGateway.Users.Events;
using LearningLantern.ApiGateway.Utility;
using LearningLantern.Common.Response;
using LearningLantern.EventBus;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace LearningLantern.ApiGateway.Users.Commands;

public class SignupDTO
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
}

public class SignupCommand : IRequest<Response<TokenResponseDTO>>, IHaveEmail, IHavePassword, IHavePersonName
{
    public SignupCommand(SignupDTO signupDTO)
    {
        Email = signupDTO.Email;
        Password = signupDTO.Password;
        FirstName = signupDTO.FirstName;
        LastName = signupDTO.LastName;
    }

    public string Email { get; set; }
    public string Password { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
}

public class SignupCommandValidator : AbstractValidator<SignupCommand>
{
    public SignupCommandValidator()
    {
        Include(new EmailValidator());
        Include(new PasswordValidator());
        Include(new PersonNameValidator());
    }
}

public class SignupCommandHandler : IRequestHandler<SignupCommand, Response<TokenResponseDTO>>
{
    private readonly IEventBus _eventBus;
    private readonly JWTGenerator _jwtGenerator;
    private readonly IMapper _mapper;
    private readonly UserManager<UserModel> _userManager;

    public SignupCommandHandler(
        IEventBus eventBus, IMapper mapper, UserManager<UserModel> userManager, JWTGenerator jwtGenerator)
    {
        _eventBus = eventBus;
        _mapper = mapper;
        _userManager = userManager;
        _jwtGenerator = jwtGenerator;
    }

    public async Task<Response<TokenResponseDTO>> Handle(SignupCommand request, CancellationToken cancellationToken)
    {
        var user = _mapper.Map<UserModel>(request);
        user.FirstName = request.FirstName.Trim();
        user.LastName = request.LastName.Trim();

        var createAsyncResult = await _userManager.CreateAsync(user, request.Password);

        if (createAsyncResult.Succeeded == false) return createAsyncResult.ToApplicationResponse<TokenResponseDTO>();

        _eventBus.Publish(_mapper.Map<UserEvent>(user));

        var result = await _jwtGenerator.GenerateJwtSecurityToken(user);

        return ResponseFactory.Ok(result);
    }
}