using System.ComponentModel.DataAnnotations;

namespace LearningLantern.Video.Data.Models;

public class VideoModel : VideoProperties
{
    public VideoModel()
    {
    }

    public VideoModel(string userId, AddVideoDTO video)
    {
        UserId = userId;
        Name = video.Name;
        ClassroomId = video.ClassroomId;
    }

    [Key] [Required] public int Id { get; set; }

    [Required] public string UserId { get; set; } = null!;
}