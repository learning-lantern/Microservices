using System.ComponentModel.DataAnnotations;

namespace LearningLantern.Video.Data.Models;

public class AddVideoDTO
{
    [Required] public int ClassroomId { get; set; }
    [Required] [StringLength(450)] public string Name { get; set; } = null!;
    [Required] public IFormFile File { get; set; } = null!;
}