using LearningLantern.ApiGateway.Classroom.Commands;
using LearningLantern.Common.Extensions;
using LearningLantern.EventBus.Events;
using MediatR;

namespace LearningLantern.ApiGateway.Classroom.Events;

public class JoinRoomEvent : IntegrationEvent
{
    public string ClassId { get; set; } = null!;
    public string UserId { get; set; } = null!;
}

public class JoinRoomEventHandler : IIntegrationEventHandler<JoinRoomEvent>
{
    private readonly ILogger<JoinRoomEventHandler> _logger;
    private readonly IMediator _mediator;

    public JoinRoomEventHandler(ILogger<JoinRoomEventHandler> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    public async Task Handle(JoinRoomEvent @event)
    {
        _logger.LogInformation("JoinRoomEventHandler = " + @event.ToJsonStringContent());
        var response = await _mediator.Send(new AddUserToClassroomCommand
        {
            ClassroomId = @event.ClassId,
            UserId = @event.UserId
        });

        if (response.Succeeded) return;

        _logger.LogError(response.ToJsonStringContent());
        throw new Exception();
    }
}