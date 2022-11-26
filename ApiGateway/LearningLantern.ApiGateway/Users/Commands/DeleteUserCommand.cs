using FluentValidation;
using LearningLantern.ApiGateway.Data.Models;
using LearningLantern.ApiGateway.Users.BuildingBlocks;
using LearningLantern.ApiGateway.Users.Events;
using LearningLantern.Common.Extensions;
using LearningLantern.Common.Response;
using LearningLantern.EventBus.Publisher;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace LearningLantern.ApiGateway.Users.Commands;

public class DeleteUserDTO
{
    public string Password { get; set; }
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
    private readonly ILogger<DeleteUserCommandHandler> _logger;
    private readonly UserManager<UserModel> _userManager;


    public DeleteUserCommandHandler(
        IEventBus eventBus, UserManager<UserModel> userManager, ILogger<DeleteUserCommandHandler> logger)
    {
        _eventBus = eventBus;
        _userManager = userManager;
        _logger = logger;
    }

    public async Task<Response> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var result = await _userManager.DeleteAsync(request.User);

        try
        {
            if (result.Succeeded)
                _eventBus.Publish(new DeleteUserEvent {Id = request.User.Id});
        }
        catch (Exception e)
        {
            _logger.LogCritical($"Can't publish the event in {nameof(DeleteUserCommandHandler)}, Exception");
            _logger.LogCritical(e.Message);
        }

        return result.ToApplicationResponse();
    }
}