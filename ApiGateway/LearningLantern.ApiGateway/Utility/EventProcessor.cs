using LearningLantern.ApiGateway.Classroom.Events;
using LearningLantern.EventBus.EventProcessor;
using LearningLantern.EventBus.Events;
using LearningLantern.EventBus.Exceptions;
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

    public async Task ProcessEvent(string eventName, string message)
    {
        _logger.LogDebug($"{eventName} with message= {message}");

        switch (eventName)
        {
            case "newRoom":
                await ProcessEvent<NewRoomEvent>(message);
                break;
            case "joinRoom":
                await ProcessEvent<JoinRoomEvent>(message);
                break;
            default:
                throw new UnhandledEventException(eventName);
        }
    }

    async private Task ProcessEvent<T>(string message)
        where T : IntegrationEvent
    {
        using var scope = _scopeFactory.CreateScope();
        var handler = scope.ServiceProvider.GetRequiredService<IIntegrationEventHandler<T>>();
        var @event = JsonConvert.DeserializeObject<T>(message);
        if (@event is not null) await handler.Handle(@event);
    }
}