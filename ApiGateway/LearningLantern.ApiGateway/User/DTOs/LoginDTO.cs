using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using LearningLantern.ApiGateway.Utility;

namespace LearningLantern.ApiGateway.User.DTOs;

public class LoginDTO
{
    [Required] [EmailAddress]
    public string Email { get; set; } = null!;

    [Required] [RegularExpression(Pattern.Password)]
    public string Password { get; set; } = null!;
}