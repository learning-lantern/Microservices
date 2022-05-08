using LearningLantern.Common.Response;

namespace LearningLantern.Calendar.Utility;

public static class ErrorsList
{
    public static Error EventNotFound(int eventId) =>
        new() {ErrorCode = nameof(EventNotFound), Description = $"Event {eventId} is not Found"};
}