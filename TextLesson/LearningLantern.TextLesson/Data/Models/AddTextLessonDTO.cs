using System.ComponentModel.DataAnnotations;

namespace LearningLantern.TextLesson.Data.Models;

public class AddTextLessonDTO
{
    [Required] public string ClassroomId { get; set; } = null!;
    [Required] public string Title { get; set; } = null!;
}