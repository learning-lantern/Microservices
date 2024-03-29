using LearningLantern.ApiGateway.Data;
using Microsoft.EntityFrameworkCore;

namespace LearningLantern.ApiGateway.Tests.Helpers;

public class LearningLanternContextMock : LearningLanternContext
{
    public LearningLanternContextMock(DbContextOptions option) : base(option)
    {
    }

    public int CountCalls { get; internal set; }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        CountCalls++;
        return base.SaveChangesAsync(cancellationToken);
    }
}