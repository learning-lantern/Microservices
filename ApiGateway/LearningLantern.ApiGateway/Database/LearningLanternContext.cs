using LearningLantern.ApiGateway.Classroom.Models;
using LearningLantern.ApiGateway.User.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LearningLantern.ApiGateway.Database;

/// <summary>
///     User data context class for Entity Framework Core.
/// </summary>
public class LearningLanternContext : IdentityDbContext<UserModel>
{
    public LearningLanternContext(DbContextOptions option) : base(option)
    {
    }

    public DbSet<ClassroomModel> Classrooms { get; set; } = null!;
    public DbSet<ClassroomUserModel> ClassroomUsers { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<ClassroomUserModel>()
            .HasKey(classroomUserModel => new {classroomUserModel.ClassroomId, classroomUserModel.UserId});

        builder.Entity<ClassroomUserModel>()
            .HasOne(classroomUserModel => classroomUserModel.User)
            .WithMany(userModel => userModel.ClassroomUsers)
            .HasForeignKey(classroomUserModel => classroomUserModel.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<ClassroomUserModel>()
            .HasOne(classroomUserModel => classroomUserModel.Classroom)
            .WithMany(classroomModel => classroomModel.ClassroomUsers)
            .HasForeignKey(classroomUserModel => classroomUserModel.ClassroomId)
            .OnDelete(DeleteBehavior.Cascade);

        base.OnModelCreating(builder);
    }
}