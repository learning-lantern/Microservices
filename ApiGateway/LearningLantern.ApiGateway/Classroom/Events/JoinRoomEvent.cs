using LearningLantern.Common.Extensions;
using LearningLantern.EventBus.Events;

namespace LearningLantern.ApiGateway.Classroom.Events;

public class JoinRoomEvent : IntegrationEvent
{
    public string ClassId { get; set; } = null!;
    public string UserId { get; set; } = null!;
}

public class JoinRoomEventHandler : IIntegrationEventHandler<JoinRoomEvent>
{
    private readonly ILogger<JoinRoomEventHandler> _logger;

    public JoinRoomEventHandler(ILogger<JoinRoomEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(JoinRoomEvent @event)
    {
        _logger.LogInformation("JoinRoomEventHandler = " + @event.ToJsonStringContent());
        return Task.CompletedTask;
    }
}