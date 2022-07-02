using System.ComponentModel.DataAnnotations;

namespace LearningLantern.Calendar.Data.Models;

public class EventProperties
{
    [Required] [StringLength(450)] public string Title { get; set; } = null!;
    public string? Description { get; set; }
    [Required] public DateTime StartTime { get; set; }
    [Required] public DateTime EndTime { get; set; }
}