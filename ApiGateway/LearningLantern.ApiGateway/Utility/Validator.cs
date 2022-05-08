using LearningLantern.ApiGateway.Data.DTOs;
using LearningLantern.Common.Response;

namespace LearningLantern.ApiGateway.Utility;

public static class Validator
{
    private static readonly List<string> Universities = new()
    {
        "Assiut University"
    };

    public static List<Error> ValidateSignUpDTO(SignUpDTO signUpDTO)
    {
        var errors = new List<Error>();

        if (IsValidUniversity(signUpDTO.University) == false)
            errors.Add(ErrorsList.UniversityNotFound());

        if (IsValidName(signUpDTO.FirstName) == false || IsValidName(signUpDTO.LastName) == false)
            errors.Add(ErrorsList.NameNotValid());

        return errors;
    }

    public static List<Error> ValidateSignInDTO(SignInDTO signInDTO)
    {
        var errors = new List<Error>();
        if (IsValidUniversity(signInDTO.University) == false)
            errors.Add(ErrorsList.UniversityNotFound());
        return errors;
    }

    private static bool IsValidUniversity(string universityName)
        => Universities.Any(university => university == universityName);

    private static bool IsValidName(string name) => name.Replace(" ", "").Length >= 2;
}