namespace LearningLantern.Common.Result;

public static class ResultFactory
{
    public static Result<T> Ok<T>(T? data = default) => new(true, data, default);

    public static Result<T> Fail<T>(Error error, T data)
    {
        return new Result<T>(false, data, new[] {error});
    }

    public static Result Ok() => new(true, default);

    public static Result Fail(Error error)
    {
        return new Result(false, new[] {error});
    }
}

public class Result
{
    public Result(bool succeeded, IEnumerable<Error>? errors)
    {
        Succeeded = succeeded;
        Errors = errors;
    }

    public bool Succeeded { get; }
    public IEnumerable<Error>? Errors { get; }
}

public class Result<T> : Result
{
    public Result(bool succeeded, T? data, IEnumerable<Error>? errors)
        : base(succeeded, errors)
    {
        Data = data;
    }

    public T? Data { get; }
}