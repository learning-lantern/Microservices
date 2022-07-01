using FluentAssertions;
using LearningLantern.TodoList.Data.Models;
using LearningLantern.TodoList.Tests.Helpers;
using Xunit;

namespace LearningLantern.TodoList.Tests.TodoRepositoryTests;

public class TodoRepositoryAddAsyncTests : TodoRepositoryTestSetup
{
    private readonly TaskProperties _randomTask;
    private readonly string _userId;

    public TodoRepositoryAddAsyncTests()
    {
        _userId = Helper.GenerateRandomUserId();
        _randomTask = Helper.GenerateTaskProperties();
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
        taskModel.Should().BeEquivalentTo(response.Data!);
        taskModel.Should().BeEquivalentTo(_randomTask);
    }

    [Fact]
    public async void ShouldReturnSameTempId()
    {
        //arrange
        //act
        var response = await TodoRepository.AddAsync(_userId, _randomTask);
        //assert
        response.Succeeded.Should().BeTrue();
    }
}