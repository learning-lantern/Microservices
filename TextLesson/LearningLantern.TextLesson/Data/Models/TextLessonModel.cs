using System.ComponentModel.DataAnnotations;

namespace LearningLantern.TextLesson.Data.Models;

public class TextLessonModel
{
    [Key] [Required] public int Id { get; set; }
    [Required] public string BlobName { get; set; } = null!;
    [Required] public string Path { get; set; } = null!;
}