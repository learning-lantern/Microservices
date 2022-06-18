using Microsoft.AspNetCore.Http;

namespace LearningLantern.Common.Response;

public abstract class ErrorBase
{
    public int StatusCode { get; init; }
}

public class Error : ErrorBase
{
    public string Description { get; init; } = null!;
    public string ErrorCode { get; init; } = null!;
}

public class ValidationError : ErrorBase
{
    public ValidationError(string propertyName, List<string> errorMessages)
    {
        PropertyName = propertyName;
        ErrorMessages = errorMessages;
        StatusCode = StatusCodes.Status400BadRequest;
    }

    public string PropertyName { get; init; }
    public List<string> ErrorMessages { get; init; }
}