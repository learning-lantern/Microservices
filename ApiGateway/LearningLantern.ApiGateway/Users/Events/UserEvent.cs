using LearningLantern.EventBus.Events;

namespace LearningLantern.ApiGateway.Users.Events;

public class UserEvent : IntegrationEvent
{
    public string Id { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;
}