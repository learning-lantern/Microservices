using LearningLantern.Common.EventBus.EventProcessor;
using LearningLantern.Common.EventBus.Events;
using Newtonsoft.Json;

namespace LearningLantern.ApiGateway.Utility;

public class EventProcessor : IEventProcessor
{
    private readonly IServiceScopeFactory _scopeFactory;

    public EventProcessor(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public Task ProcessEvent(string eventName, string message)
    {
        return eventName switch
        {
            _ => Task.CompletedTask
        };
    }

    private Task ProcessEvent<T>(string message)
        where T : IntegrationEvent
    {
        using var scope = _scopeFactory.CreateScope();
        var handler = scope.ServiceProvider.GetRequiredService<IIntegrationEventHandler<T>>();
        var @event = JsonConvert.DeserializeObject<T>(message);
        return @event is not null ? handler.Handle(@event) : Task.CompletedTask;
    }
}