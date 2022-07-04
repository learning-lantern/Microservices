using System.ComponentModel.DataAnnotations;

namespace LearningLantern.Video.Data.Models;

public class AddVideoDTO
{
    public IFormFile? File { get; set; }
    public List<VideoQuiz> QuizList { get; set; } = new List<VideoQuiz>();
}