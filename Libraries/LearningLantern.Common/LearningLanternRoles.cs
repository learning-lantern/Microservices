namespace LearningLantern.Common;

public static class LearningLanternRoles
{
    public const string Admin = "Admin";
    public const string Instructor = "Instructor";
    public const string UniversityAdmin = "UniversityAdmin";
    public const string Student = "Student";

    public static IEnumerable<string> AllRoles = new[]
    {
        Admin, Instructor, UniversityAdmin, Student
    };
}