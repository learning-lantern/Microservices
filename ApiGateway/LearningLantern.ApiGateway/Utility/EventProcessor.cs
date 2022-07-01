using LearningLantern.ApiGateway.Classroom.Events;
using LearningLantern.Common.EventBus.EventProcessor;
using LearningLantern.Common.EventBus.Events;
using Newtonsoft.Json;

namespace LearningLantern.ApiGateway.Utility;

public class EventProcessor : IEventProcessor
{
    private readonly IServiceScopeFactory _scopeFactory;
    private ILogger<EventProcessor> _logger;

    public EventProcessor(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;

        using var scope = scopeFactory.CreateScope();

        _logger = scope.ServiceProvider.GetRequiredService<ILogger<EventProcessor>>();
    }

    public Task ProcessEvent(string eventName, string message)
    {
        return eventName switch
        {
            "newRoom" => ProcessEvent<NewRoomEvent>(message),
            _ => throw new Exception()
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