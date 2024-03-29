﻿using System.ComponentModel.DataAnnotations;
using LearningLantern.ApiGateway.Utility;

namespace LearningLantern.ApiGateway.Data.DTOs;

/// <summary>
///     User data transfer object class.
/// </summary>
public class UserDTO
{
    [Required] public string Id { get; set; } = null!;

    [Required] [EmailAddress] public string Email { get; set; } = null!;

    [Required] [StringLength(30)] [RegularExpression(Pattern.Name)]
    public string FirstName { get; set; } = null!;

    [Required] [StringLength(30)] [RegularExpression(Pattern.Name)]
    public string LastName { get; set; } = null!;
}