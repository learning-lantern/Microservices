using System.ComponentModel.DataAnnotations;

namespace LearningLantern.TextLesson.Data.Models;

public class UpdateTextLessonDTO
{
    [Required] public int Id { get; set; }
    public string HtmlBody { get; set; } = string.Empty;
}