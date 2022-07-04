using System.ComponentModel.DataAnnotations;

namespace LearningLantern.TextLesson.Data.Models;

public class TextLessonDTO
{
    [Key] [Required] public int Id { get; set; }
    [Required] public string Title { get; set; } = null!;
    public string HtmlBody { get; set; } = string.Empty;
}