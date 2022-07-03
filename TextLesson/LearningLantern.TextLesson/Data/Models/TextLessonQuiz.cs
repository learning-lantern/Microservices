using Newtonsoft.Json;

namespace LearningLantern.TextLesson.Data.Models;

public class TextLessonQuiz
{
    [JsonIgnore]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public int QuizId { get; set; }
    public int Time { get; set; }
}