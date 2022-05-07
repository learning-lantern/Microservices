using System.ComponentModel.DataAnnotations;

namespace LearningLantern.Common.Models.CalendarModels;

public class UpdateEventDTO
{
    [Required] [StringLength(450)] public string Title { get; set; } = null!;
    public string? Description { get; set; }
    [Required] public DateTime StartTime { get; set; }
    [Required] public DateTime EndTime { get; set; }
}