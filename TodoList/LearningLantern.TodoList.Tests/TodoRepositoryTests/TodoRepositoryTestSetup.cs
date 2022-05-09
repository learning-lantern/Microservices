using System;
using LearningLantern.TodoList.Repositories;

namespace LearningLantern.TodoList.Tests.TodoRepositoryTests;

public abstract class TodoRepositoryTestSetup : IDisposable
{
    protected readonly TodoContextMock Context;
    protected readonly TodoRepository TodoRepository;

    protected TodoRepositoryTestSetup()
    {
        var mapper = Helper.CreateAutoMapper();
        Context = Helper.CreateTodoContextMock(Guid.NewGuid().ToString());
        TodoRepository = new TodoRepository(Context, mapper);
    }

    public void Dispose()
    {
        Context.Database.EnsureDeleted();
        Context.Dispose();
    }
}