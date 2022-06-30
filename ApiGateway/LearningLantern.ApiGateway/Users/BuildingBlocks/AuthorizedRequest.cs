using LearningLantern.ApiGateway.Users.Models;
using MediatR;

namespace LearningLantern.ApiGateway.Users.BuildingBlocks;

public abstract class AuthorizedRequest<TResponse> : IRequest<TResponse>
{
    public UserModel User { get; set; } = null!;
}

public abstract class AuthorizedWithPasswordRequest<TResponse> : AuthorizedRequest<TResponse>, IHavePassword
{
    public new UserModel User { get; set; } = null!;
    public string Password { get; set; } = null!;
}