using FluentValidation.Results;
using LearningLantern.Common.Responses;

namespace LearningLantern.Common.Extensions;

public static class ValidationFailureExtensions
{
    public static IEnumerable<ValidationError> ToApplicationValidationError(this IEnumerable<ValidationFailure> failure) => failure.GroupBy(x => x.ErrorCode, x => x.ErrorMessage).Select(x => new ValidationError(x.Key, x.ToList()));
}