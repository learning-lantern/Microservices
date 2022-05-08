using System.ComponentModel.DataAnnotations;

namespace LearningLantern.Calendar.Data.Models;

public class EventDTO : UpdateEventDTO
{
    [Required] public int ClassroomId { get; set; }
}