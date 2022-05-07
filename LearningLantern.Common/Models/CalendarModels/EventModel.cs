namespace LearningLantern.Common.Models.CalendarModels;

public class EventModel : EventDTO, IEntity
{
    public int Id { get; set; }

    public void Update(UpdateEventDTO updateEventDTO)
    {
        Title = updateEventDTO.Title;
        Description = updateEventDTO.Description;
        StartTime = updateEventDTO.StartTime;
        EndTime = updateEventDTO.EndTime;
    }
}