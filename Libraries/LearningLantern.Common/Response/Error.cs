using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace LearningLantern.Common.Response;

public class Error
{
    [JsonIgnore] public int StatusCode { get; init; }
    public string ErrorCode { get; init; } = null!;
    public string Description { get; init; } = null!;
}

public static class ValidationFailureExtensions
{
    public static IEnumerable<Error> ToApplicationValidationError(this IEnumerable<ValidationFailure> failure)
    {
        return failure.Select(x =>
            new Error
            {
                StatusCode = StatusCodes.Status400BadRequest,
                ErrorCode = x.PropertyName + "NotValid",
                Description = x.ErrorMessage
            }
        );
    }
}