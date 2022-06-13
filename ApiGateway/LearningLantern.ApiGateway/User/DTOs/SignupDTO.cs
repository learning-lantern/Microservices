using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using LearningLantern.ApiGateway.Utility;

namespace LearningLantern.ApiGateway.User.DTOs;

public class SignupDTO : LoginDTO
{
    [Required] [StringLength(30)] [RegularExpression(Pattern.Name)] [JsonPropertyName("userFName")]
    public string FirstName { get; set; } = null!;

    [Required] [StringLength(30)] [RegularExpression(Pattern.Name)] [JsonPropertyName("userLName")]
    public string LastName { get; set; } = null!;
}