using Newtonsoft.Json;

namespace LearningLantern.Common.Response;

public class Error
{
    [JsonIgnore] public int StatusCode { get; init; } = 0;

    public string ErrorCode { get; init; }
    public string Description { get; init; }
}