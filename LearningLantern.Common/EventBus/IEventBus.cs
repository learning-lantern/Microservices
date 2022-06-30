using LearningLantern.Common.EventBus.Events;

namespace LearningLantern.Common.EventBus;

public interface IEventBus
{
    public bool SetupConfiguration();
    
    void Publish(IntegrationEvent @event);

    public void AddEvent<T>(string queueName)
        where T : IntegrationEvent;

    public void Subscribe(string queueName);
}