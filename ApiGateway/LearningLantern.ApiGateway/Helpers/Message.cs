using System.Net;
using System.Net.Mail;

namespace LearningLantern.ApiGateway.Helpers;

public static class Message
{
    public const string NameNotValid =
        "If the name has spaces, then the length of his alphabetic characters must be greater than or equal to 2.";

    public const string TaskNotFound = "There is no task with this Id.";
    public const string TaskRemoved = "The task was deleted successfully.";

    public const string QuizNotFound = "There is no quiz with this Id.";
    public const string QuizRemoved = "The quiz was deleted successfully.";

    public const string ClassroomNotFound = "There is no classroom with this Id.";
    public const string ClassroomRemoved = "The classroom was deleted successfully.";

    public const string EventNotFound = "There is no event with this Id.";
    public const string EventRemoved = "The event was deleted successfully.";

    public const string UserDeleted = "The user was deleted successfully.";
    public const string UserIdNotFound = "There is no user in this University with this Id.";
    public const string UserEmailNotFound = "There is no user in this University with this email.";

    public const string UniversityNotFound = "There is no University in our database with this name.";

    public const string FromEmailPassword = "LL@gmail22";
    public const string FromEmailHost = "smtp.gmail.com";
    public const string EmailSubject = "Confirmation Email";
    public const string FromEmail = "learning.lantern.group@gmail.com";

    public static readonly SmtpClient Client = new()
    {
        Host = FromEmailHost,
        Port = 587,
        EnableSsl = true,
        Credentials = new NetworkCredential(
            FromEmail,
            FromEmailPassword)
    };

    public static string EmailBody(string userId, string token) =>
        $"<h1>Welcome To Learning Lantern</h1><br><p> Thanks for registering at learning lantern please click <strong><a href=\"https://localhost:5001/api/Auth/ConfirmEmail?userId={userId}&token={token}\" target=\"_blank\">here</a></strong> to activate your account</p>";

    public static string UpdateUserRole(string userId, string role) => $"Updated the user: {userId} role to {role}.";

    public static string AddClassroomUser(string userId, int classroomId) =>
        $"The user: {userId} added to classroom {classroomId}.";

    public static string RemoveClassroomUser(string userId, int classroomId) =>
        $"The user: {userId} removed from classroom {classroomId}.";

    public static string CreatedRole(string role) => $"Created the {role} role.";
}