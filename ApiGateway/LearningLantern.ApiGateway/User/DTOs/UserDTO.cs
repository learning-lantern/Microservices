using System.ComponentModel.DataAnnotations;
using LearningLantern.ApiGateway.Helpers;
using LearningLantern.ApiGateway.User.Models;

namespace LearningLantern.ApiGateway.User.DTOs;

/// <summary>
///     User data transfare object class.
/// </summary>
public class UserDTO
{
    public UserDTO()
    {
    }

    public UserDTO(UserDTO userDTO)
    {
        Id = userDTO.Id;
        Email = userDTO.Email;
        FirstName = userDTO.FirstName;
        LastName = userDTO.LastName;
        University = userDTO.University;
    }

    public UserDTO(UserModel userModel)
    {
        Id = userModel.Id;
        Email = userModel.Email;
        FirstName = userModel.FirstName;
        LastName = userModel.LastName;
        University = "Assiut University";
    }

    [Required] public string Id { get; set; } = null!;

    [Required] [EmailAddress] public string Email { get; set; } = null!;

    [Required] [StringLength(30)] [RegularExpression(Pattern.Name)]
    public string FirstName { get; set; } = null!;

    [Required] [StringLength(30)] [RegularExpression(Pattern.Name)]
    public string LastName { get; set; } = null!;

    [Required] public string University { get; set; } = null!;
}