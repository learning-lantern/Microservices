using System.Threading.Tasks;
using LearningLantern.Common.Response;
using LearningLantern.TodoList.Tests.Helpers;
using LearningLantern.TodoList.Utility;
using Xunit;

namespace LearningLantern.TodoList.Tests.TodoRepositoryTests;

public class TodoRepositoryRemoveAsyncTests : TodoRepositoryTestSetup
{
    async private Task<int> AddTaskToRepository()
    {
        var userId = Helper.GenerateRandomUserId();
        var randomTask = Helper.GenerateAddTaskDTO();
        var response = await TodoRepository.AddAsync(userId, randomTask);
        var id = response.Data!.Task.Id;
        Context.CountCalls = 0;
        return id;
    }

    [Fact]
    public async void ShouldCallSaveChangesOnlyOneWhenFound()
    {
        //arrange
        var id = await AddTaskToRepository();
        //act
        await TodoRepository.RemoveAsync(id);
        //assert
        Assert.Equal(1, Context.CountCalls);
    }

    [Fact]
    public async void ShouldCallSaveChangesZeroTimesWhenNotFound()
    {
        //arrange
        //act
        var response = await TodoRepository.RemoveAsync(123);
        //assert
        Assert.Equal(0, Context.CountCalls);
        Assert.False(response.Succeeded);
    }

    [Fact]
    public async void TestRemoveTaskWhenFound()
    {
        //arrange
        var id = await AddTaskToRepository();
        //act
        var response = await TodoRepository.RemoveAsync(id);
        //assert
        Assert.True(response.Succeeded);
        Assert.Empty(Context.Tasks);
    }

    [Fact]
    public async void TestRemoveTaskWhenNotFound()
    {
        //arrange
        //act
        var response = await TodoRepository.RemoveAsync(123);
        //assert
        Assert.False(response.Succeeded);
        Assert.NotNull(response.Errors);
        Assert.Contains(response.Errors!, e => e is Error {ErrorCode: nameof(ErrorsList.TaskNotFound)});
    }
}