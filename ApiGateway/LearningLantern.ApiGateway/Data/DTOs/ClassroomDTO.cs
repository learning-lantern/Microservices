using System.ComponentModel.DataAnnotations;

namespace LearningLantern.ApiGateway.Data.DTOs;

public class ClassroomDTO : AddClassroomDTO
{
    [Key] [Required] public int Id { get; set; }
}