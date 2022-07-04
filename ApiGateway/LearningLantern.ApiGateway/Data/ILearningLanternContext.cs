using LearningLantern.ApiGateway.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace LearningLantern.ApiGateway.Data;

public interface ILearningLanternContext
{
    public DbSet<ClassroomModel> Classrooms { get; set; }
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}