using LearningLantern.ApiGateway.Classroom.DTOs;

namespace LearningLantern.ApiGateway.Classroom.Models;

public class ClassroomModel : ClassroomDTO
{
    public ClassroomModel()
    {
    }

    public ClassroomModel(ClassroomDTO classroomDTO) : base(classroomDTO)
    {
    }

    public ClassroomModel(AddClassroomDTO addClassroomDTO) : base(addClassroomDTO)
    {
    }

    public ICollection<ClassroomUserModel> ClassroomUsers { get; set; } = null!;
}