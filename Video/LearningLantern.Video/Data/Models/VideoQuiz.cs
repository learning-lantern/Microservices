using Newtonsoft.Json;

namespace LearningLantern.Video.Data.Models;

public class VideoQuiz
{
    [JsonIgnore]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public int QuizId { get; set; }
    public int Time { get; set; }
}