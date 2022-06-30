using FluentAssertions;
using LearningLantern.TodoList.Data.Models;
using LearningLantern.TodoList.Tests.Helpers;
using Xunit;

namespace LearningLantern.TodoList.Tests.TodoRepositoryTests;

public class TodoRepositoryAddAsyncTests : TodoRepositoryTestSetup
{
    private readonly AddTaskDTO _randomTask;
    private readonly string _userId;

    public TodoRepositoryAddAsyncTests()
    {
        _userId = Helper.GenerateRandomUserId();
        _randomTask = Helper.GenerateAddTaskDTO();
    }

    [Fact]
    public async void ShouldCallSaveChangesOnlyOnce()
    {
        //arrange
        //act
        await TodoRepository.AddAsync(_userId, _randomTask);
        //assert
        Assert.Equal(1, Context.CountCalls);
    }

    [Fact]
    public async void TestAddTask()
    {
        //arrange
        //act
        var response = await TodoRepository.AddAsync(_userId, _randomTask);
        //assert
        var taskModel = Assert.Single(Context.Tasks);
        taskModel.Should().BeEquivalentTo(response.Data!.Task);
        taskModel.Should().BeEquivalentTo(_randomTask,
            options => options.Excluding(x => x.TempId));
    }

    [Fact]
    public async void ShouldReturnSameTempId()
    {
        //arrange
        //act
        var response = await TodoRepository.AddAsync(_userId, _randomTask);
        //assert
        Assert.True(response.Succeeded);
        Assert.Equal(response.Data!.TempId, _randomTask.TempId);
    }
}