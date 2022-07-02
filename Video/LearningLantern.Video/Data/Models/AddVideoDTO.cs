using System.ComponentModel.DataAnnotations;

namespace LearningLantern.Video.Data.Models;

public class AddVideoDTO
{
    [Required] public IFormFile File { get; set; } = null!;
    [Required] public List<VideoQuiz> QuizList { get; set; } = null!;
}