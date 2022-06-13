using System.ComponentModel.DataAnnotations;
using LearningLantern.ApiGateway.Utility;

namespace LearningLantern.ApiGateway.User.DTOs;

public class UpdatePasswordDTO
{
    [Required] [RegularExpression(Pattern.Password)]
    public string OldPassword { get; set; } = null!;

    [Required] [RegularExpression(Pattern.Password)]
    public string NewPassword { get; set; } = null!;
}