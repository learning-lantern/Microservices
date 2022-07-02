using System.ComponentModel.DataAnnotations;

namespace LearningLantern.Calendar.Data.Models;

public class EventModel : AddEventDTO
{
    [Required] [Key] public int Id { get; set; }

    public void Update(EventProperties eventProperties)
    {
        Title = eventProperties.Title;
        Description = eventProperties.Description;
        StartDate = eventProperties.StartDate;
        DueDate = eventProperties.DueDate;
    }
}