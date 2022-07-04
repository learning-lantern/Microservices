using System.ComponentModel.DataAnnotations;

namespace LearningLantern.Calendar.Data.Models;

public class AddEventDTO : EventProperties
{
    [Required] public string ClassroomId { get; set; }
}