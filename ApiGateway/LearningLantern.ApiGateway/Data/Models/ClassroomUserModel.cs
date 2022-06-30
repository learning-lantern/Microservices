namespace LearningLantern.ApiGateway.Data.Models;

public class ClassroomUserModel
{
    public ClassroomUserModel()
    {
    }

    public ClassroomUserModel(int classroomId, string userId)
    {
        ClassroomId = classroomId;
        UserId = userId;
    }

    public int ClassroomId { get; set; }
    public ClassroomModel Classroom { get; set; } = null!;
    public string UserId { get; set; } = null!;
    public UserModel User { get; set; } = null!;
}