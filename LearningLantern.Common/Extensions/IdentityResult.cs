using LearningLantern.Common.Response;
using Microsoft.AspNetCore.Identity;

namespace LearningLantern.Common.Extensions;

public static class IdentityResultExtensions
{
    public static Response.Response ToApplicationResponse(this IdentityResult identityResult)
    {
        var errors = identityResult.Errors.Select(e => new Error { ErrorCode = e.Code, Description = e.Description });
        return new(identityResult.Succeeded, errors);
    }

    public static Response<T> ToApplicationResponse<T>(this IdentityResult identityResult, T? data = default)
    {
        var errors = identityResult.Errors.Select(e => new Error { ErrorCode = e.Code, Description = e.Description });
        return new(identityResult.Succeeded, data, errors);
    }

    public static Response.Response ToApplicationResponse(this SignInResult signInResult) => new(signInResult.Succeeded, default);

    public static Response<T> ToApplicationResponse<T>(this SignInResult signInResult, T? data = default) => new(signInResult.Succeeded, data, default);
}