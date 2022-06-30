using AutoMapper;
using LearningLantern.ApiGateway.Users.Events;
using LearningLantern.ApiGateway.Users.Models;
using LearningLantern.ApiGateway.Utility;
using LearningLantern.Common.EventBus;
using LearningLantern.Common.Extensions;
using LearningLantern.Common.Responses;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace LearningLantern.ApiGateway.Users.Commands;

public class ConfirmNewEmailCommand : IRequest<Response>
{
    public string UserId { get; set; } = null!;
    public string Token { get; set; } = null!;
    public string Email { get; set; } = null!;
}

public class ConfirmNewEmailCommandHandler : IRequestHandler<ConfirmNewEmailCommand, Response>
{
    private readonly IEventBus eventBus;
    private readonly IMapper mapper;
    private readonly UserManager<UserModel> userManager;

    public ConfirmNewEmailCommandHandler(IEventBus eventBus, IMapper mapper, UserManager<UserModel> userManager)
    {
        this.eventBus = eventBus;
        this.mapper = mapper;
        this.userManager = userManager;
    }

    public async Task<Response> Handle(ConfirmNewEmailCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(request.UserId);
        if (user is null) return ResponseFactory.Fail(ErrorsList.UserIdNotFound(request.UserId));

        var result = await userManager.ChangeEmailAsync(user, request.Email, request.Token);

        if (result.Succeeded) eventBus.Publish(mapper.Map<UpdateUserEvent>(user));

        return result.ToApplicationResponse();
    }
}