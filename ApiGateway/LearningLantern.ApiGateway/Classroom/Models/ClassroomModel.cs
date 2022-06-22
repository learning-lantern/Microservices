using LearningLantern.ApiGateway.Classroom.DTOs;
using LearningLantern.ApiGateway.Users.Models;

namespace LearningLantern.ApiGateway.Classroom.Models;

public class ClassroomModel : ClassroomDTO
{
    public ICollection<UserModel> Users { get; set; } = null!;

    public void Update(AddClassroomDTO addClassroomDTO)
    {
        Name = addClassroomDTO.Name;
        Description = addClassroomDTO.Description;
    }
}