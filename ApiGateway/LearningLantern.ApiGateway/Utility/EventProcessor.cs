using System.Text.Json;
using LearningLantern.Common.EventBus.EventProcessor;
using LearningLantern.Common.EventBus.Events;

namespace LearningLantern.ApiGateway.Utility;

public class EventProcessor : IEventProcessor
{
    private readonly IServiceScopeFactory scopeFactory;

    public EventProcessor(IServiceScopeFactory scopeFactory)
    {
        this.scopeFactory = scopeFactory;
    }

    public Task ProcessEvent(string eventName, string message)
    {
        return eventName switch
        {
            _ => Task.CompletedTask
        };
    }

    private Task ProcessEvent<T>(string message) where T : IntegrationEvent
    {
        using var scope = scopeFactory.CreateScope();
        var handler = scope.ServiceProvider.GetRequiredService<IIntegrationEventHandler<T>>();
        var @event = JsonSerializer.Deserialize<T>(message);
        return @event is not null ? handler.Handle(@event) : Task.CompletedTask;
    }
}