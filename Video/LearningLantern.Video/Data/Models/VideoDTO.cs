using System.ComponentModel.DataAnnotations;

namespace LearningLantern.Video.Data.Models;

public class VideoDTO
{
    [Key] [Required] public int Id { get; set; }
    [Required] public string Path { get; set; } = null!;
    [Required] public List<VideoQuiz> QuizList { get; set; } = null!;
}