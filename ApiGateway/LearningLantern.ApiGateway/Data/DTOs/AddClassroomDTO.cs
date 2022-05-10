using System.ComponentModel.DataAnnotations;

namespace LearningLantern.ApiGateway.Data.DTOs;

public class AddClassroomDTO
{
    [Required] [StringLength(30)] public string Name { get; set; } = null!;
    public string? Description { get; set; }
}