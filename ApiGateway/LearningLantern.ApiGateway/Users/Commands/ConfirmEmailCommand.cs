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

public class ConfirmEmailCommand : IRequest<Response>
{
    public string UserId { get; set; } = null!;
    public string Token { get; set; } = null!;
}

public class ConfirmEmailHandler : IRequestHandler<ConfirmEmailCommand, Response>
{
    private readonly IEventBus _eventBus;
    private readonly IMapper _mapper;
    private readonly UserManager<UserModel> _userManager;

    public ConfirmEmailHandler(IEventBus eventBus, IMapper mapper, UserManager<UserModel> userManager)
    {
        _eventBus = eventBus;
        _mapper = mapper;
        _userManager = userManager;
    }

    public async Task<Response> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId);
        if (user is null) return ResponseFactory.Fail(ErrorsList.UserIdNotFound(request.UserId));

        var result = await _userManager.ConfirmEmailAsync(user, request.Token);

        if (result.Succeeded) _eventBus.Publish(_mapper.Map<UpdateUserEvent>(user));

        return result.ToApplicationResponse();
    }
}