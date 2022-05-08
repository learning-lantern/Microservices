using LearningLantern.Common.Response;

namespace LearningLantern.TodoList.Utility;

public static class ErrorsList
{
    public static Error TaskNotFound(int taskId) =>
        new() {ErrorCode = nameof(TaskNotFound), Description = $"Task {taskId} is not Found"};
}