using System.ComponentModel.DataAnnotations;

namespace LearningLantern.ApiGateway.Data.DTOs;

public class AddClassroomDTO
{
    public AddClassroomDTO()
    {
    }

    public AddClassroomDTO(AddClassroomDTO addClassroomDTO)
    {
        Name = addClassroomDTO.Name;
        Description = addClassroomDTO.Description;
    }

    [Required] [StringLength(30)] public string Name { get; set; } = null!;

    public string? Description { get; set; }
}