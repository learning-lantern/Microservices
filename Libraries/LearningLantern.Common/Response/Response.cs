namespace LearningLantern.Common.Response;

public static class ResponseFactory
{
    public static Response Ok() => new(true, default);
    public static Response<T> Ok<T>(T? data = default) => new(true, data, default);

    public static Response Fail() => new(false, default);
    public static Response Fail(Error error) => new(false, new[] {error});
    public static Response Fail(IEnumerable<Error> errors) => new(false, errors);
    public static Response<T> Fail<T>() => new(false, default, default);
    public static Response<T> Fail<T>(Error error, T? data = default) => Fail(new[] {error}, data);
    public static Response<T> Fail<T>(IEnumerable<Error> errors, T? data = default) => new(false, data, errors);

    public static TResponse? CreateFailObject<TResponse>(IEnumerable<Error> failures)
        where TResponse : Response
    {
        var responseType = typeof(TResponse);
        if (!responseType.IsGenericType) return Activator.CreateInstance(responseType, false, failures) as TResponse;
        var resultType = responseType.GetGenericArguments()[0];
        var invalidResponseType = typeof(TResponse).MakeGenericType(resultType);
        return Activator.CreateInstance(invalidResponseType, false, null, failures) as TResponse;
    }
}

public class Response
{
    public Response(bool succeeded, IEnumerable<Error>? errors)
    {
        Succeeded = succeeded;
        Errors = errors;
    }

    public bool Succeeded { get; }
    public IEnumerable<Error>? Errors { get; }
}

public class Response<T> : Response
{
    public Response(bool succeeded, T? data, IEnumerable<Error>? errors)
        : base(succeeded, errors)
    {
        Data = data;
    }

    public T? Data { get; }
}