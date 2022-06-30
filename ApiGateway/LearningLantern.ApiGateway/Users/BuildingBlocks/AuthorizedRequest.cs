using LearningLantern.ApiGateway.Data.Models;
using MediatR;

namespace LearningLantern.ApiGateway.Users.BuildingBlocks;

public abstract class AuthorizedRequest<TResponse> : IRequest<TResponse>
{
    public UserModel User { get; set; }
}

public abstract class AuthorizedWithPasswordRequest<TResponse> : AuthorizedRequest<TResponse>, IHavePassword
{
    public new UserModel User { get; set; }
    public string Password { get; set; }
}