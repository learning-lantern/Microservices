using System.ComponentModel.DataAnnotations;

namespace LearningLantern.Video.Data.Models;

public class AddVideoDTO
{
    [Required] public IFormFile File { get; set; } = null!;
    public string? QuizList { get; set; } = string.Empty;

    //public List<VideoQuiz> QuizList { get; set; } = new();
}