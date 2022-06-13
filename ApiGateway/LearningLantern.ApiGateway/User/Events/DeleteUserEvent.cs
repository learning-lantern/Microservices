using LearningLantern.Common.EventBus.Events;

namespace LearningLantern.ApiGateway.User.Events;

public class DeleteUserEvent : IntegrationEvent
{
    public string Id { get; set; } = null!;
}