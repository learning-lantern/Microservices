using System.ComponentModel.DataAnnotations;

namespace LearningLantern.Video.Data.Models;

public class VideoModel : VideoProperties
{
    [Key]
    [Required]
    public int Id { get; set; }

    [Required]
    public string UserId { get; set; } = null!;

    public VideoModel() { }
    public VideoModel(string userId, AddVideoDTO video) : base(video)
    {
        UserId = userId;
    }
}