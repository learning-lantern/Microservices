using System.ComponentModel.DataAnnotations;

namespace LearningLantern.ApiGateway.Data.Models;

public class ClassroomModel
{
    [Key] [Required] public string Id { get; set; }
    [StringLength(30)] public string? Name { get; set; }
    public string? Description { get; set; }
    public ICollection<UserModel> Users { get; set; }
}