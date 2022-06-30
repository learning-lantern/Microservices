using LearningLantern.ApiGateway.Data.Models;
using LearningLantern.ApiGateway.Users.BuildingBlocks;
using LearningLantern.ApiGateway.Utility;
using LearningLantern.Common.Response;
using LearningLantern.Common.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace LearningLantern.ApiGateway.PipelineBehaviors;

public class AuthorizedBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : AuthorizedRequest<TResponse>
    where TResponse : Response
{
    private readonly ICurrentUserService _currentUserService;
    private readonly UserManager<UserModel> _userManager;

    public AuthorizedBehavior(ICurrentUserService currentUserService, UserManager<UserModel> userManager)
    {
        _currentUserService = currentUserService;
        _userManager = userManager;
    }

    public async Task<TResponse> Handle(
        TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        var userId = _currentUserService.UserId!;
        var user = await _userManager.FindByIdAsync(userId);

        var failures = new List<ErrorBase>();

        if (user is null)
        {
            failures.Add(ErrorsList.UserIdNotFound(userId));
        }
        else if (request is AuthorizedWithPasswordRequest<TResponse> passwordRequest)
        {
            var password = passwordRequest.Password;
            if (await _userManager.CheckPasswordAsync(user, password) == false)
                failures.Add(ErrorsList.IncorrectPassword(userId));
        }

        if (failures.Any()) return ResponseFactory.CreateFailObject<TResponse>(failures)!;

        request.User = user!;
        return await next();
    }
}