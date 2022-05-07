using System.ComponentModel.DataAnnotations;

namespace LearningLantern.ApiGateway.Classroom.DTOs;

public class ClassroomDTO : AddClassroomDTO
{
    public ClassroomDTO()
    {
    }

    public ClassroomDTO(AddClassroomDTO addTaskDTO) : base(addTaskDTO)
    {
    }

    public ClassroomDTO(ClassroomDTO classroomDTO) : base(classroomDTO)
    {
        Id = classroomDTO.Id;
    }

    [Required] [Key] public int Id { get; set; }
}