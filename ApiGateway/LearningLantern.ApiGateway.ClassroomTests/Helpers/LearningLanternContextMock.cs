using System;
using System.Threading;
using System.Threading.Tasks;
using LearningLantern.ApiGateway.Data;
using LearningLantern.ApiGateway.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace LearningLantern.ApiGateway.ClassroomTests.Helpers;

public class LearningLanternContextMock : LearningLanternContext, ILearningLanternContext
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
    
}