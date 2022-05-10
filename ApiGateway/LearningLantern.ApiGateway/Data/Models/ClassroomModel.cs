using LearningLantern.ApiGateway.Data.DTOs;

namespace LearningLantern.ApiGateway.Data.Models;

public class ClassroomModel : ClassroomDTO
{
    public ICollection<ClassroomUserModel> ClassroomUsers { get; set; } = null!;

    public void Update(AddClassroomDTO addClassroomDTO)
    {
        Name = addClassroomDTO.Name;
        Description = addClassroomDTO.Description;
    }
}