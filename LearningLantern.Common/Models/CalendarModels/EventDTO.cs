using System.ComponentModel.DataAnnotations;

namespace LearningLantern.Common.Models.CalendarModels;

public class EventDTO : UpdateEventDTO
{
    [Required] public int ClassroomId { get; set; }
}