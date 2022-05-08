using LearningLantern.ApiGateway.Data.DTOs;

namespace LearningLantern.ApiGateway.Data.Models;

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