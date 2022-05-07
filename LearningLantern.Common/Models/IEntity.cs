using System.ComponentModel.DataAnnotations;

namespace LearningLantern.Common.Models;

public interface IEntity
{
    [Required] [Key] public int Id { get; set; }
}