using System.ComponentModel.DataAnnotations;

namespace LearningLantern.Video.Data.Models;

public class AddVideoDTO : VideoProperties
{
    [Required]
    public string Path { get; set; } = null!;

    [Required]
    public string Type { get; set; } = null!;


    public AddVideoDTO() { }

    public AddVideoDTO(AddVideoDTO video) : base(video)
    {
        Path = video.Path;
        Type = video.Type;
    }
}