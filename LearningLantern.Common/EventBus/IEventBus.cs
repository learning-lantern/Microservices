using LearningLantern.Common.EventBus.Events;

namespace LearningLantern.Common.EventBus;

public interface IEventBus
{
    void Publish(IntegrationEvent @event);

    public void Subscribe<T>(string queueName)
        where T : IntegrationEvent;
}