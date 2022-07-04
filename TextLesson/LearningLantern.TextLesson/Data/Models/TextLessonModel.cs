using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace LearningLantern.TextLesson.Data.Models;

public class TextLessonModel
{
    [Key] [Required] public int Id { get; set; }
    [Required] public string Title { get; set; } = null!;
    [Required] public string BlobName { get; set; } = null!;
}