using System.ComponentModel.DataAnnotations;

namespace LearningLantern.TextLesson.Data.Models;

public class TextLessonDTO
{
    [Required] public string Id { get; set; } = null!;
    [Required] public string Title { get; set; } = null!;
}