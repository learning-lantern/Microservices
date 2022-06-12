using System.Text.RegularExpressions;
using LearningLantern.ApiGateway.Configurations;
using LearningLantern.ApiGateway.Data.DTOs;
using LearningLantern.Common.Response;

namespace LearningLantern.ApiGateway.Utility;

public static class Validator
{
    public static List<Error> Validate(SignUpDTO signUpDTO)
    {
        var errors = new List<Error>();

        if (IsValidName(signUpDTO.FirstName) == false || IsValidName(signUpDTO.LastName) == false)
            errors.Add(ErrorsList.NameNotValid());

        return errors;
    }

    private static bool IsValidName(string name) => new Regex(Pattern.Name).IsMatch(name.Replace(" ", ""));
}