using System;
using LearningLantern.ApiGateway.Classroom.Repositories;
using MediatR;
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
        var mediator = new Mock<IMediator>();
        ClassroomRepository = new ClassroomRepository(Context, mapper, mediator.Object);
    }

    public void Dispose()
    {
        Context.Database.EnsureDeleted();
        Context.Dispose();
    }
}