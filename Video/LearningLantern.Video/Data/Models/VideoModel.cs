using System.ComponentModel.DataAnnotations;
using LearningLantern.Video.Data.Models;

public class VideoModel : VideoProperties
{
    [Key]
    [Required]
    public int Id { get; set; }

    [Required]
    public string UserId { get; set; } = null!;

    public VideoModel() { }
    public VideoModel(string userId, AddVideoDTO video)
    {
        UserId = userId;
        Name = video.Name;
        ClassroomId = video.ClassroomId;
    }
}