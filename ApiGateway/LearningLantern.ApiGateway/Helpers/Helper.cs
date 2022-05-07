using System.Text.RegularExpressions;
using LearningLantern.ApiGateway.Configurations;

namespace LearningLantern.ApiGateway.Helpers;

public static class Helper
{
    public static readonly Regex PasswordRegex = new(Pattern.Password);

    private static readonly List<string> Universities = new()
    {
        "Assiut University"
    };

    public static bool IsUniversityValid(string universityName)
    {
        foreach (var university in Universities)
            if (university == universityName)
                return true;

        return false;
    }

    public static bool IsNameValid(string name) => name.Replace(" ", "").Length >= 2;
}