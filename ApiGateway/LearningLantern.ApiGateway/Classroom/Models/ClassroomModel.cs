using LearningLantern.ApiGateway.Classroom.DTOs;

namespace LearningLantern.ApiGateway.Classroom.Models;

public class ClassroomModel : ClassroomDTO
{
    public ICollection<ClassroomUserModel> ClassroomUsers { get; set; } = null!;

    public void Update(AddClassroomDTO addClassroomDTO)
    {
        Name = addClassroomDTO.Name;
        Description = addClassroomDTO.Description;
    }
}