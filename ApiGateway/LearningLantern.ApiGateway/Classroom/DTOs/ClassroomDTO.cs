using System.ComponentModel.DataAnnotations;

namespace LearningLantern.ApiGateway.Classroom.DTOs;

public class ClassroomDTO : AddClassroomDTO
{
    [Key] [Required] public int Id { get; set; }
}