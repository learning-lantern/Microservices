using LearningLantern.Common.Result;

namespace LearningLantern.TodoList.Utility;

public static class ErrorsList
{
    public static Error TaskNotFound(int eventId) =>
        new() {Code = "TaskNotFound", Description = $"Task {eventId} is not Found"};
}