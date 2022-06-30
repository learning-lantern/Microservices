using FluentValidation;
using LearningLantern.ApiGateway.Users.BuildingBlocks;
using LearningLantern.ApiGateway.Users.Events;
using LearningLantern.ApiGateway.Users.Models;
using LearningLantern.Common.EventBus;
using LearningLantern.Common.Extensions;
using LearningLantern.Common.Responses;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace LearningLantern.ApiGateway.Users.Commands;

public class DeleteUserDTO
{
    public string Password { get; set; } = null!;
}

public class DeleteUserCommand : AuthorizedWithPasswordRequest<Response>
{
    public DeleteUserCommand(DeleteUserDTO deleteUserDTO)
    {
        Password = deleteUserDTO.Password;
    }
}

public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
{
    public DeleteUserCommandValidator()
    {
        Include(new PasswordValidator());
    }
}

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Response>
{
    private readonly IEventBus _eventBus;
    private readonly UserManager<UserModel> _userManager;


    public DeleteUserCommandHandler(IEventBus eventBus, UserManager<UserModel> userManager)
    {
        _eventBus = eventBus;
        _userManager = userManager;
    }

    public async Task<Response> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var result = await _userManager.DeleteAsync(request.User);

        if (result.Succeeded) _eventBus.Publish(new DeleteUserEvent {Id = request.User.Id});

        return result.ToApplicationResponse();
    }
}