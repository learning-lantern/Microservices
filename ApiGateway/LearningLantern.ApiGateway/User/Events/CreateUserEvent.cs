using LearningLantern.Common.EventBus.Events;

namespace LearningLantern.ApiGateway.User.Events;

public class CreateUserEvent : IntegrationEvent
{
    public string Id { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;
}