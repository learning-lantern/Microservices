using System.ComponentModel.DataAnnotations;

namespace LearningLantern.TextLesson.Data.Models;

public class TextLessonModel
{
    [Key] [Required] public int Id { get; set; }
    [Required] public string ClassroomId { get; set; } = null!;
    [Required] public string Title { get; set; } = null!;
    public string HtmlBody { get; set; } = string.Empty;
}