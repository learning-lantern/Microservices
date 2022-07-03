using System.ComponentModel.DataAnnotations;

namespace LearningLantern.TextLesson.Data.Models;

public class AddTextLessonDTO
{
    [Required] public IFormFile File { get; set; } = null!;
    public List<TextLessonQuiz> QuizList { get; set; } = new List<TextLessonQuiz>();
}