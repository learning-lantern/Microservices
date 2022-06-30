using System.ComponentModel.DataAnnotations;

namespace LearningLantern.Video.Data.Models;

public class AddVideoDTO : VideoProperties
{
    [Required]
    public string Path { get; set; } = null!;

    public AddVideoDTO(AddVideoDTO video) : base(video)
    {
        Path = video.Path;
    }
}