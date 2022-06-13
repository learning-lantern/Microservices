using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using LearningLantern.ApiGateway.Utility;

namespace LearningLantern.ApiGateway.User.DTOs;

public class UpdateEmailDTO
{
    [Required] [EmailAddress] [JsonPropertyName("userEmail")]
    public string Email { get; set; } = null!;

    [Required] [RegularExpression(Pattern.Password)] [JsonPropertyName("userPassword")]
    public string Password { get; set; } = null!;
}