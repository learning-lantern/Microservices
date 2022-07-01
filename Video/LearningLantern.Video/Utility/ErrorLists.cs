using LearningLantern.Common.Response;

namespace LearningLantern.Video.Utility;

public static class ErrorsList
{
    public static Error VideoNotFound(int videoId) => new()
    {
        StatusCode = StatusCodes.Status404NotFound,
        ErrorCode = nameof(VideoNotFound),
        Description = $"Video {videoId} is not Found"
    };
}