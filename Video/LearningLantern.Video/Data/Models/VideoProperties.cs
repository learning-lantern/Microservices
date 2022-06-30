using System.ComponentModel.DataAnnotations;

namespace LearningLantern.Video.Data.Models;

public class VideoProperties
{
    [Required][StringLength(450)] public String Path { get; set; } = null!;
    [Required][StringLength(450)] public string Name { get; set; } = null!;
    [Required][StringLength(450)] public string Type { get; set; } = null!;

    public string? Discription { get; set; }
}