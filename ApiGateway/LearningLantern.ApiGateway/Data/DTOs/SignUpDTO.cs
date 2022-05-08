using System.ComponentModel.DataAnnotations;
using LearningLantern.ApiGateway.Configurations;

namespace LearningLantern.ApiGateway.Data.DTOs;

/// <summary>
///     Create data transfer object class, inherits from "SignInDTO" class.
/// </summary>
public class SignUpDTO : SignInDTO
{
    [Required] [StringLength(30)] [RegularExpression(Pattern.Name)]
    public string FirstName { get; set; } = null!;

    [Required] [StringLength(30)] [RegularExpression(Pattern.Name)]
    public string LastName { get; set; } = null!;
}