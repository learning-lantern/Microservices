using LearningLantern.ApiGateway.Data.Models;
using LearningLantern.ApiGateway.Users.BuildingBlocks;
using LearningLantern.ApiGateway.Utility;
using LearningLantern.Common.Response;
using LearningLantern.Common.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace LearningLantern.ApiGateway.PipelineBehaviors;

public class AuthorizedBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
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
        if (request is not AuthorizedRequest<TResponse> authorizedRequest) return await next();

        var userId = _currentUserService.UserId!;
        var user = await _userManager.FindByIdAsync(userId);

        var failures = new List<Error>();

        if (user is null)
        {
            failures.Add(ErrorsList.UserIdNotFound(userId));
        }
        else if (authorizedRequest is AuthorizedWithPasswordRequest<TResponse> passwordRequest)
        {
            var password = passwordRequest.Password;
            if (await _userManager.CheckPasswordAsync(user, password) == false)
                failures.Add(ErrorsList.IncorrectPassword(userId));
        }

        if (failures.Any()) return ResponseFactory.CreateFailObject<TResponse>(failures)!;

        var prop = request.GetType().GetProperties();

        foreach (var x in prop)
            if (x.Name == "User")
                x.SetValue(request, user);

        return await next();
    }
}