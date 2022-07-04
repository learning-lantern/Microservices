using LearningLantern.EventBus.Events;

namespace LearningLantern.EventBus.Publisher;

public interface IEventBus
{
    public bool SetupConfiguration();

    void Publish(IntegrationEvent @event);

    public void AddEvent<T>(string queueName)
        where T : IntegrationEvent;
}