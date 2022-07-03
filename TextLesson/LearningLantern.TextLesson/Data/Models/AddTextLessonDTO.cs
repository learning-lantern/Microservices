using System.ComponentModel.DataAnnotations;

namespace LearningLantern.TextLesson.Data.Models;

public class AddTextLessonDTO
{
    [Required] public IFormFile File { get; set; } = null!;
}