using System.ComponentModel.DataAnnotations;

namespace LearningLantern.Video.Data.Models;

public class VideoProperties
{
    [Required]
    [StringLength(450)]
    public string Name { get; set; } = null!;

    [Required]
    public int ClassroomId { get; set; }

    public VideoProperties() { }

    public VideoProperties(VideoProperties video)
    {
        Name = video.Name;
        ClassroomId = video.ClassroomId;
    }
}