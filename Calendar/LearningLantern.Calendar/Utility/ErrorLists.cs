using LearningLantern.Common.Result;

namespace LearningLantern.Calendar.Utility;

public static class ErrorsList
{
    public static Error EventNotFound(int eventId) =>
        new() {Code = "EventNotFound", Description = $"Event {eventId} is not Found"};
}