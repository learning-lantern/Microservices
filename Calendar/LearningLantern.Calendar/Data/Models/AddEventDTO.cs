using System.ComponentModel.DataAnnotations;

namespace LearningLantern.Calendar.Data.Models;

public class AddEventDTO : EventProperties
{
    [Required] public int ClassroomId { get; set; }
}