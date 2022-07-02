using System.ComponentModel.DataAnnotations;

namespace LearningLantern.TextLesson.Data.Models;

public class AddTextLessonDTO
{
    [Required] public int ClassroomId { get; set; }
    [Required] [StringLength(450)] public string Name { get; set; } = null!;
    [Required] public IFormFile File { get; set; } = null!;
}