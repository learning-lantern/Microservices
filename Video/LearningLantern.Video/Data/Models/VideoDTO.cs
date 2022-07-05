using System.ComponentModel.DataAnnotations;

namespace LearningLantern.Video.Data.Models;

public class VideoDTO
{
    public VideoDTO()
    {
    }

    public VideoDTO(VideoModel videoModel)
    {
        Id = videoModel.Id;
        Path = videoModel.Path;
        QuizList.AddRange(videoModel.QuizList);
    }

    [Key] [Required] public int Id { get; set; }
    [Required] public string Path { get; set; } = null!;
    public List<VideoQuiz> QuizList { get; set; } = new();
}