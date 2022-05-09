using System.ComponentModel.DataAnnotations;
using LearningLantern.Common;

namespace LearningLantern.TodoList.Data.Models;

public class TaskProperties : ValueObject
{
    [Required] [StringLength(450)] public string Title { get; set; } = null!;
    public string? Note { get; set; }
    public DateTime? DueDate { get; set; }
    public bool MyDay { get; set; }
    public bool Completed { get; set; }
    public bool Important { get; set; }
    public int Repeated { get; set; }

    public override IEnumerable<object?> GetEqualityComponents()
        => new object?[] {Title, Note, DueDate, MyDay, Important, Repeated};
}