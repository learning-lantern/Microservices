using LearningLantern.ApiGateway.Classroom.Commands;
using LearningLantern.Common.Extensions;
using LearningLantern.EventBus.Events;
using MediatR;

namespace LearningLantern.ApiGateway.Classroom.Events;

public class NewRoomEvent : IntegrationEvent
{
    public string ClassId { get; set; } = null!;
    public string UserId { get; set; } = null!;
}

public class NewRoomEventHandler : IIntegrationEventHandler<NewRoomEvent>
{
    private readonly ILogger<NewRoomEventHandler> _logger;
    private readonly IMediator _mediator;

    public NewRoomEventHandler(ILogger<NewRoomEventHandler> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    public async Task Handle(NewRoomEvent @event)
    {
        var response = await _mediator.Send(new CreateNewClassroomCommand
        {
            ClassroomId = @event.ClassId
        });

        if (response.Succeeded == false)
        {
            _logger.LogError(@event.ToJsonStringContent());
            return;
        }

        response = await _mediator.Send(new AddUserToClassroomCommand
        {
            ClassroomId = @event.ClassId,
            UserId = @event.UserId
        });
        if (response.Succeeded) return;
        _logger.LogError(response.ToJsonStringContent());
        throw new Exception();
    }
}