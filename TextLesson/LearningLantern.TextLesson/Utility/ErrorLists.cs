using LearningLantern.Common.Response;

namespace LearningLantern.TextLesson.Utility;

public static class ErrorsList
{
    public static Error TextLessonNotFound(int textLessonId) => new()
    {
        StatusCode = StatusCodes.Status404NotFound,
        ErrorCode = nameof(TextLessonNotFound),
        Description = $"TextLesson {textLessonId} is not Found."
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

    public static Error CantDeleteTextLesson() => new()
    {
        StatusCode = StatusCodes.Status400BadRequest,
        ErrorCode = nameof(CantDeleteTextLesson),
        Description = "Can't Delete the file from the storage. Please try again later."
    };
}