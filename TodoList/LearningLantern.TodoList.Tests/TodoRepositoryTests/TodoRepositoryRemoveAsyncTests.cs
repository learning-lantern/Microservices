using Xunit;

namespace LearningLantern.TodoList.Tests.TodoRepositoryTests;

public class TodoRepositoryRemoveAsyncTests : TodoRepositoryTestSetup
{
    [Fact]
    public async void ShouldCallSaveChangesOnlyOneWhenFound()
    {
        //arrange
        var userId = Helper.GenerateRandomUserId();
        var randomTask = Helper.GenerateAddTaskDTO();
        var response = await TodoRepository.AddAsync(userId, randomTask);
        var id = response.Data!.Task.Id;
        Context.CountCalls = 0;
        //act
        await TodoRepository.RemoveAsync(id);
        //assert
        Assert.Equal(1, Context.CountCalls);
    }

    [Fact]
    public async void ShouldCallSaveChangesOnlyZeroWhenNotFound()
    {
        //arrange
        //act
        var response = await TodoRepository.RemoveAsync(123);
        //assert
        Assert.Equal(0, Context.CountCalls);
        Assert.False(response.Succeeded);
    }

    [Fact]
    public async void TestRemoveTask()
    {
        //arrange
        var userId = Helper.GenerateRandomUserId();
        var randomTask = Helper.GenerateAddTaskDTO();
        var result = await TodoRepository.AddAsync(userId, randomTask);
        var id = result.Data!.Task.Id;
        Context.CountCalls = 0;
        //act
        var response = await TodoRepository.RemoveAsync(id);
        //assert
        Assert.True(response.Succeeded);
        Assert.Empty(Context.Tasks);
    }
}