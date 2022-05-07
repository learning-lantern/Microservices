using System.ComponentModel.DataAnnotations;
using LearningLantern.ApiGateway.Configurations;

namespace LearningLantern.ApiGateway.Auth.DTOs;

/// <summary>
///     Create data transfare object class, inherits from "SignInDTO" class.
/// </summary>
public class CreateDTO : SignInDTO
{
    [Required] [StringLength(30)] [RegularExpression(Pattern.Name)]
    public string FirstName { get; set; } = null!;

    [Required] [StringLength(30)] [RegularExpression(Pattern.Name)]
    public string LastName { get; set; } = null!;
}