using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace LearningLantern.Common.Response;

public abstract class ErrorBase
{
    [JsonIgnore] public int StatusCode { get; init; }
}

public class Error : ErrorBase
{
    public string Description { get; init; }
    public string ErrorCode { get; init; }
}

public class ValidationError : ErrorBase
{
    public ValidationError()
    {
        StatusCode = StatusCodes.Status400BadRequest;
    }

    public string PropertyName { get; init; }
    public List<string> ErrorMessages { get; init; }
}

public static class ValidationFailureExtensions
{
    public static IEnumerable<ValidationError> ToApplicationValidationError(this IEnumerable<ValidationFailure> failure)
    {
        return failure.GroupBy(x => x.ErrorCode, x => x.ErrorMessage).Select(x =>
            new ValidationError
            {
                PropertyName = x.Key,
                ErrorMessages = x.ToList()
            }
        );
    }
}