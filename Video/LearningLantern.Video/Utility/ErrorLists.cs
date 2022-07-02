using LearningLantern.Common.Response;

namespace LearningLantern.Video.Utility;

public static class ErrorsList
{
    public static Error VideoNotFound(int videoId) => new()
    {
        StatusCode = StatusCodes.Status404NotFound,
        ErrorCode = nameof(VideoNotFound),
        Description = $"Video {videoId} is not Found."
    };
    public static Error CantDownloadFile() => new()
    {
        StatusCode = StatusCodes.Status400BadRequest,
        ErrorCode = nameof(CantDownloadFile),
        Description = "Can't Download the file from our storage."
    };

    public static Error CantUploadFile() => new()
    {
        StatusCode = StatusCodes.Status400BadRequest,
        ErrorCode = nameof(CantUploadFile),
        Description = "Can't Upload the file to our storage. Maybe the file is very large."
    };

    public static Error CantDeleteVideo() => new()
    {
        StatusCode = StatusCodes.Status400BadRequest,
        ErrorCode = nameof(CantDeleteVideo),
        Description = "Can't Delete the file from the storage. Please try again later."
    };
}