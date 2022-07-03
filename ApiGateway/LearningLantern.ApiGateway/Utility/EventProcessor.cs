using LearningLantern.ApiGateway.Classroom.Events;
using LearningLantern.EventBus;
using LearningLantern.EventBus.EventProcessor;
using LearningLantern.EventBus.Events;
using Newtonsoft.Json;

namespace LearningLantern.ApiGateway.Utility;

public class EventProcessor : IEventProcessor
{
    private readonly ILogger<EventProcessor> _logger;
    private readonly IServiceScopeFactory _scopeFactory;

    public EventProcessor(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;

        using var scope = scopeFactory.CreateScope();

        _logger = scope.ServiceProvider.GetRequiredService<ILogger<EventProcessor>>();
    }

    public Task ProcessEvent(string eventName, string message)
    {
        _logger.LogDebug($"{eventName} with message= {message}");
        return eventName switch
        {
            "newRoom" => ProcessEvent<NewRoomEvent>(message),
            "joinRoom" => ProcessEvent<JoinRoomEvent>(message),
            _ => throw new UnhandledEventException()
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