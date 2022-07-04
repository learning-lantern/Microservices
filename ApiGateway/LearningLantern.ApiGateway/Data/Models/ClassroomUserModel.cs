namespace LearningLantern.ApiGateway.Data.Models;

public class ClassroomUserModel
{
    public string ClassroomId { get; set; }
    public ClassroomModel Classroom { get; set; } = null!;
    public string UserId { get; set; } = null!;
    public UserModel User { get; set; } = null!;
}