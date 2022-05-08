namespace LearningLantern.ApiGateway.Helpers;

public static class Message
{
    public const string NameNotValid =
        "If the name has spaces, then the length of his alphabetic characters must be greater than or equal to 2.";


    public const string ClassroomNotFound = "There is no classroom with this Id.";
    public const string ClassroomRemoved = "The classroom was deleted successfully.";

    public const string UserIdNotFound = "There is no user in this University with this Id.";

    public static string AddClassroomUser(string userId, int classroomId) =>
        $"The user: {userId} added to classroom {classroomId}.";

    public static string RemoveClassroomUser(string userId, int classroomId) =>
        $"The user: {userId} removed from classroom {classroomId}.";

    public static string CreatedRole(string role) => $"Created the {role} role.";
}