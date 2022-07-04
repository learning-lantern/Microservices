using System.ComponentModel.DataAnnotations;

namespace LearningLantern.TextLesson.Data.Models;

public class AddTextLessonDTO
{
    [Required] public string Id { get; set; } = null!;
    [Required] public IFormFile File { get; set; } = null!;
}