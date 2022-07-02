using System.ComponentModel.DataAnnotations;

namespace LearningLantern.Calendar.Data.Models;

public class EventDTO : EventProperties
{
    [Required] [Key] public int Id { get; set; }
}