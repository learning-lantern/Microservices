using System.ComponentModel.DataAnnotations;

namespace LearningLantern.Video.Data.Models;

public class AddVideoDTO : VideoProperties
{
    public AddVideoDTO()
    {
    }

    public AddVideoDTO(AddVideoDTO video) : base(video)
    {
        Path = video.Path;
    }

    [Required] public string Path { get; set; } = null!;
}