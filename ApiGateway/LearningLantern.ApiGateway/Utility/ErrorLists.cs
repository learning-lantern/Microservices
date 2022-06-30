using LearningLantern.Common.Response;

namespace LearningLantern.ApiGateway.Utility;

public static class ErrorsList
{
    public static Error UserEmailNotFound(string email) =>
        new()
        {
            StatusCode = StatusCodes.Status404NotFound,
            ErrorCode = nameof(UserEmailNotFound),
            Description = $"There is no user in this University with this email {email}."
        };

    public static Error UserIdNotFound(string userId) =>
        new()
        {
            StatusCode = StatusCodes.Status404NotFound,
            ErrorCode = nameof(UserIdNotFound),
            Description = $"There is no user in this University with this Id {userId}."
        };

    public static Error IncorrectPassword(string userId) =>
        new()
        {
            StatusCode = StatusCodes.Status403Forbidden,
            ErrorCode = nameof(IncorrectPassword),
            Description = $"this is incorrect password for this user {userId}"
        };

    public static Error SignInNotAllowed() =>
        new()
        {
            StatusCode = StatusCodes.Status400BadRequest,
            ErrorCode = nameof(SignInNotAllowed),
            Description = "SignIn not allowed"
        };

    public static Error SignInFailed() =>
        new()
        {
            StatusCode = StatusCodes.Status400BadRequest,
            ErrorCode = nameof(SignInFailed),
            Description = "SignIn Failed, Wrong username or password."
        };

    public static Error NameNotValid() => new()
    {
        StatusCode = StatusCodes.Status400BadRequest,
        ErrorCode = nameof(NameNotValid),
        Description =
            "this name is not valid, the length of the alphabetic characters must be greater than or equal to 2."
    };

    public static Error ClassroomIdNotFound(int classroomId) => new()
    {
        StatusCode = StatusCodes.Status404NotFound,
        ErrorCode = nameof(ClassroomIdNotFound),
        Description = $"classroom {classroomId} is not Found"
    };
}