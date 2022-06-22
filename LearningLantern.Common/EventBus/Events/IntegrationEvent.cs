using Newtonsoft.Json;

namespace LearningLantern.Common.EventBus.Events;

public class IntegrationEvent
{
    public IntegrationEvent()
    {
        EventId = Guid.NewGuid();
        CreationDate = DateTime.UtcNow;
    }

    [JsonConstructor]
    public IntegrationEvent(Guid eventId, DateTime createDate)
    {
        EventId = eventId;
        CreationDate = createDate;
    }

    [JsonProperty]
    public Guid EventId { get; }

    [JsonProperty]
    public DateTime CreationDate { get; }
}