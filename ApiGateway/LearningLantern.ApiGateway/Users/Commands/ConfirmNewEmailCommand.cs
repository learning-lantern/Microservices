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
    public string UserId { get; set; }
    public string Token { get; set; }
    public string Email { get; set; }
}

public class ConfirmNewEmailCommandHandler : IRequestHandler<ConfirmNewEmailCommand, Response>
{
    private readonly IEventBus _eventBus;
    private readonly IMapper _mapper;
    private readonly UserManager<UserModel> _userManager;

    public ConfirmNewEmailCommandHandler(IEventBus eventBus, IMapper mapper, UserManager<UserModel> userManager)
    {
        _eventBus = eventBus;
        _mapper = mapper;
        _userManager = userManager;
    }

    public async Task<Response> Handle(ConfirmNewEmailCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId);
        if (user is null) return ResponseFactory.Fail(ErrorsList.UserIdNotFound(request.UserId));

        var result = await _userManager.ChangeEmailAsync(user, request.Email, request.Token);

        if (result.Succeeded) _eventBus.Publish(_mapper.Map<UpdateUserEvent>(user));

        return result.ToApplicationResponse();
    }
}