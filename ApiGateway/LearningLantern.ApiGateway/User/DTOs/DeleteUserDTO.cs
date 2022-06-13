using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using LearningLantern.ApiGateway.Utility;

namespace LearningLantern.ApiGateway.User.DTOs;

public class DeleteUserDTO
{
    [Required] [RegularExpression(Pattern.Password)] [JsonPropertyName("userPassword")]
    public string Password { get; set; } = null!;
}