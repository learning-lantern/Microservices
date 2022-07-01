using System.Linq;
using FluentAssertions;
using LearningLantern.TodoList.Data.Models;
using LearningLantern.TodoList.Tests.Helpers;
using Xunit;

namespace LearningLantern.TodoList.Tests.TodoRepositoryTests;

public class TodoRepositoryUpdateAsyncTests : TodoRepositoryTestSetup
{
    private readonly TaskModel _taskModel;

    public TodoRepositoryUpdateAsyncTests()
    {
        var userId = Helper.GenerateRandomUserId();
        var randomTask = Helper.GenerateTaskProperties();
        var response = TodoRepository.AddAsync(userId, randomTask).Result;
        _taskModel = Context.Tasks.Single(x => x.Id == response.Data!.Id);
        Context.CountCalls = 0;
    }

    [Fact]
    public async void ShouldCallSaveChangesOnlyOne()
    {
        //arrange
        var randomTaskProperties = Helper.GenerateTaskProperties();
        //act
        await TodoRepository.UpdateAsync(_taskModel.Id, randomTaskProperties);
        //assert
        Assert.Equal(1, Context.CountCalls);
    }

    [Fact]
    public async void TestUpdateAsync()
    {
        //arrange
        var randomTaskProperties = Helper.GenerateTaskProperties();
        //act
        var response = await TodoRepository.UpdateAsync(_taskModel.Id, randomTaskProperties);
        //assert
        Assert.True(response.Succeeded);
        _taskModel.Should().BeEquivalentTo(randomTaskProperties);
    }
}