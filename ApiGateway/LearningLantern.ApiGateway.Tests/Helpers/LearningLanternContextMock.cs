using System.Threading;
using System.Threading.Tasks;
using LearningLantern.ApiGateway.Data;
using LearningLantern.ApiGateway.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace LearningLantern.ApiGateway.Tests.Helpers;

public class LearningLanternContextMock : DbContext, ILearningLanternContext
{
    public LearningLanternContextMock(DbContextOptions option) : base(option)
    {
    }

    public int CountCalls { get; internal set; }

    public DbSet<ClassroomModel> Classrooms { get; set; }
    public DbSet<ClassroomUserModel> ClassroomUsers { get; set; }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        CountCalls++;
        return base.SaveChangesAsync(cancellationToken);
    }

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