using System.ComponentModel.DataAnnotations;

namespace LearningLantern.Video.Data.Models;

public class VideoDTO
{
    [Key] [Required] public int Id { get; set; }
    [Required] [StringLength(450)] public string Name { get; set; } = null!;
}