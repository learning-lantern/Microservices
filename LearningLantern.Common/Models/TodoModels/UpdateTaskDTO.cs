using System.ComponentModel.DataAnnotations;

namespace LearningLantern.Common.Models.TodoModels;

public class UpdateTaskDTO
{
    [Required] [StringLength(450)] public string Title { get; set; } = null!;
    public string? Note { get; set; }
    public DateTime? DueDate { get; set; }
    public bool MyDay { get; set; }
    public bool Completed { get; set; }
    public bool Important { get; set; }
    public int Repeated { get; set; }
}