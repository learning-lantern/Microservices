using System.ComponentModel.DataAnnotations;

namespace LearningLantern.TextLesson.Data.Models;

public class TextLessonModel
{
    public TextLessonModel()
    {
    }

    public TextLessonModel(string userId, AddTextLessonDTO textLesson)
    {
        UserId = userId;
        Name = textLesson.Name;
        ClassroomId = textLesson.ClassroomId;
    }

    [Key] [Required] public int Id { get; set; }
    [Required] [StringLength(450)] public string Name { get; set; } = null!;

    [Required] public int ClassroomId { get; set; }
    [Required] public string UserId { get; set; } = null!;
    [Required] public string BlobName { get; set; } = null!;
}