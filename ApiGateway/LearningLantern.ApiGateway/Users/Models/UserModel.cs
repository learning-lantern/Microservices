using System.ComponentModel.DataAnnotations;
using LearningLantern.ApiGateway.Classroom.Models;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace LearningLantern.ApiGateway.Users.Models;

/// <summary>
///     User data model class, inherits from "IdentityUser" class.
/// </summary>
public class UserModel : IdentityUser
{
    [StringLength(30)] public string FirstName { get; set; } = null!;

    [StringLength(30)] public string LastName { get; set; } = null!;

    [JsonIgnore]
    public ICollection<ClassroomModel>? Classrooms { get; set; }
}