using System;
using LearningLantern.ApiGateway.Repositories;
using Moq;

namespace LearningLantern.ApiGateway.Tests.Helpers;

public abstract class ClassroomRepositoryTestSetup : IDisposable
{
    protected readonly ClassroomRepository ClassroomRepository;
    protected readonly LearningLanternContextMock Context;

    protected ClassroomRepositoryTestSetup()
    {
        var mapper = Helper.CreateAutoMapper();
        Context = Helper.LearningLanternContextMock(Guid.NewGuid().ToString());
        var identity = new Mock<IIdentityRepository>();
        ClassroomRepository = new ClassroomRepository(Context, mapper, identity.Object);
    }

    public void Dispose()
    {
        Context.Database.EnsureDeleted();
        Context.Dispose();
    }
}