using FluentAssertions;
using LearningLantern.TodoList.Data.Models;
using Xunit;

namespace LearningLantern.TodoList.Tests;

public class TaskModelTests
{
    [Fact]
    public void TestUpdateFunction()
    {
        // arrange
        var task = new TaskModel();
        var taskProperties = Helper.GenerateTaskProperties();
        // act
        task.Update(taskProperties);
        // assert
        task.Should().BeEquivalentTo(taskProperties);
    }
}