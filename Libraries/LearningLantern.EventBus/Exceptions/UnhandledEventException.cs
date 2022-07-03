namespace LearningLantern.EventBus.Exceptions;

public class UnhandledEventException : Exception
{
    public UnhandledEventException(string eventName) : base($"there is no handler for event:{eventName}")
    {
    }
}