using System.ComponentModel.DataAnnotations;

namespace LearningLantern.Video.Data.Models;

public class VideoModel
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
    [Required] [StringLength(450)] public string Name { get; set; } = null!;

    [Required] public int ClassroomId { get; set; }
    [Required] public string UserId { get; set; } = null!;
    [Required] public string BlobName { get; set; } = null!;
}