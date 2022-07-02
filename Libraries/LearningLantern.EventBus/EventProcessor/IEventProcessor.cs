namespace LearningLantern.EventBus.EventProcessor;

public interface IEventProcessor
{
    Task ProcessEvent(string eventName, string message);
}