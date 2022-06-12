using System.ComponentModel.DataAnnotations;
using LearningLantern.ApiGateway.Configurations;
using LearningLantern.ApiGateway.Data.Models;

namespace LearningLantern.ApiGateway.Data.DTOs;

/// <summary>
///     User data transfer object class.
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
    }

    public UserDTO(UserModel userModel)
    {
        Id = userModel.Id;
        Email = userModel.Email;
        FirstName = userModel.FirstName;
        LastName = userModel.LastName;
    }

    [Required] public string Id { get; set; } = null!;

    [Required] [EmailAddress] public string Email { get; set; } = null!;

    [Required] [StringLength(30)] [RegularExpression(Pattern.Name)]
    public string FirstName { get; set; } = null!;

    [Required] [StringLength(30)] [RegularExpression(Pattern.Name)]
    public string LastName { get; set; } = null!;
}