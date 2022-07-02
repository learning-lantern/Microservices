using LearningLantern.EventBus.Events;

namespace LearningLantern.ApiGateway.Users.Events;

public class DeleteUserEvent : IntegrationEvent
{
    public string Id { get; set; } = null!;
}