using LearningLantern.ApiGateway.Classroom.Models;
using LearningLantern.ApiGateway.Users.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LearningLantern.ApiGateway.Data;

/// <summary>
///     User data context class for Entity Framework Core.
/// </summary>
public class LearningLanternContext : IdentityDbContext<UserModel>, ILearningLanternContext
{
    public LearningLanternContext(DbContextOptions option) : base(option) { }

    public DbSet<ClassroomModel> Classrooms { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
}