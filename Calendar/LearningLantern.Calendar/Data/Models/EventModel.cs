using System.ComponentModel.DataAnnotations;

namespace LearningLantern.Calendar.Data.Models;

public class EventModel : EventDTO
{
    [Required] [Key] public int Id { get; set; }

    public void Update(UpdateEventDTO updateEventDTO)
    {
        Title = updateEventDTO.Title;
        Description = updateEventDTO.Description;
        StartTime = updateEventDTO.StartTime;
        EndTime = updateEventDTO.EndTime;
    }
}