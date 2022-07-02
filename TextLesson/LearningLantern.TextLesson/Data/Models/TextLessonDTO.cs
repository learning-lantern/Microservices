using System.ComponentModel.DataAnnotations;

namespace LearningLantern.TextLesson.Data.Models;

public class TextLessonDTO
{
    [Key] [Required] public int Id { get; set; }
    [Required] [StringLength(450)] public string Name { get; set; } = null!;
}