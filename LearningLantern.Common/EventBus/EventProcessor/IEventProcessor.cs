namespace LearningLantern.Common.EventBus.EventProcessor;

public interface IEventProcessor
{
    Task ProcessEvent(string eventName, string message);
}