using LearningLantern.ApiGateway.Classroom.Models;
using Microsoft.EntityFrameworkCore;

namespace LearningLantern.ApiGateway.Data;

public interface ILearningLanternContext
{
    DbSet<ClassroomModel> Classrooms { get; set; }
    DbSet<ClassroomUserModel> ClassroomUsers { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}