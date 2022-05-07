using AutoMapper;
using LearningLantern.TodoList.Exceptions;
using LearningLantern.TodoList.Repositories;
using Xunit;

namespace LearningLantern.TodoList.Tests;

public class TodoRepositoryTests
{
    private readonly IMapper _mapper;

    public TodoRepositoryTests()
    {
        _mapper = Helper.CreateAutoMapper();
    }

    [Fact]
    public async void ShouldCallSaveChangesOnlyOne_AddAsync()
    {
        //arrange
        await using var context = Helper.CreateTodoContextMock(
            nameof(ShouldCallSaveChangesOnlyOne_AddAsync));
        var todoRepository = new TodoRepository(context, _mapper);
        var task = Helper.GenerateTaskDTO();
        //act
        await todoRepository.AddAsync(task);
        //assert
        Assert.Equal(1, context.CountCalls);
    }

    [Fact]
    public async void ShouldCallSaveChangesOnlyOne_UpdateAsync()
    {
        //arrange
        await using var context = Helper.CreateTodoContextMock(
            nameof(ShouldCallSaveChangesOnlyOne_UpdateAsync));
        var todoRepository = new TodoRepository(context, _mapper);
        var taskModel = (await todoRepository.AddAsync(Helper.GenerateTaskDTO())).Data;
        context.CountCalls = 0;
        //act
        await todoRepository.UpdateAsync(taskModel.Id, taskModel);
        //assert
        Assert.Equal(1, context.CountCalls);
    }

    [Fact]
    public async void ShouldCallSaveChangesOnlyOne_RemoveAsync()
    {
        //arrange
        await using var context = Helper.CreateTodoContextMock(
            nameof(ShouldCallSaveChangesOnlyOne_RemoveAsync));
        var todoRepository = new TodoRepository(context, _mapper);
        var id = (await todoRepository.AddAsync(Helper.GenerateTaskDTO())).Data.Id;
        context.CountCalls = 0;
        //act
        await todoRepository.RemoveAsync(id);
        //assert
        Assert.Equal(1, context.CountCalls);
    }

    [Fact]
    public async void ShouldCallSaveChangesOnlyZeroWhenNotFound_RemoveAsync()
    {
        //arrange
        await using var context = Helper.CreateTodoContextMock(
            nameof(ShouldCallSaveChangesOnlyZeroWhenNotFound_RemoveAsync));
        var todoRepository = new TodoRepository(context, _mapper);
        //act
        await todoRepository.RemoveAsync(123);
        //assert
        Assert.Equal(0, context.CountCalls);
    }

    [Fact]
    public async void TestAddTask()
    {
        //arrange
        await using var context = Helper.CreateTodoContextMock(
            nameof(TestAddTask));
        TodoRepository todoRepository = new(context, _mapper);
        //act
        var response = await todoRepository.AddAsync(Helper.GenerateTaskDTO());
        //assert
        var taskModel = Assert.Single(context.Tasks);
        Assert.Equal(taskModel.Id, response.Data.Id);
    }

    [Fact]
    public async void ShouldThrowTaskNotFoundException()
    {
        //arrange
        await using var context = Helper.CreateTodoContextMock(
            nameof(ShouldThrowTaskNotFoundException));
        var todoRepository = new TodoRepository(context, _mapper);
        //act
        //assert
        await Assert.ThrowsAsync<TaskNotFoundException>(
            () => todoRepository.UpdateAsync(123, Helper.GenerateUpdateTaskDTO())
        );
    }
}