using LearningLantern.ApiGateway.Classroom.Repositories;
using LearningLantern.Common.Extensions;
using LearningLantern.EventBus.Events;

namespace LearningLantern.ApiGateway.Classroom.Events;

public class JoinRoomEvent : IntegrationEvent
{
    public string ClassId { get; set; } = null!;
    public string UserId { get; set; } = null!;
}

public class JoinRoomEventHandler : IIntegrationEventHandler<NewRoomEvent>
{
    private readonly IClassroomRepository _classroomRepository;
    private readonly ILogger<JoinRoomEventHandler> _logger;

    public JoinRoomEventHandler(ILogger<JoinRoomEventHandler> logger, IClassroomRepository classroomRepository)
    {
        _logger = logger;
        _classroomRepository = classroomRepository;
    }

    public Task Handle(NewRoomEvent @event)
    {
        _logger.LogInformation("JoinRoomEvent = " + @event.ToJsonStringContent());
        return Task.CompletedTask;
    }
}