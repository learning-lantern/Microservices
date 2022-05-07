using LearningLantern.Common.Models.TodoModels;
using Xunit;

namespace LearningLantern.TodoList.Tests;

public class TaskModelTests
{
    [Fact]
    public void TestUpdateFunction()
    {
        // arrange
        var task = new TaskModel();
        var addTaskDto = Helper.GenerateUpdateTaskDTO();
        // act
        task.Update(addTaskDto);
        // assert
        Assert.Equal(addTaskDto.Title, task.Title);
        Assert.Equal(addTaskDto.Note, task.Note);
        Assert.Equal(addTaskDto.DueDate, task.DueDate);
        Assert.Equal(addTaskDto.MyDay, task.MyDay);
        Assert.Equal(addTaskDto.Completed, task.Completed);
        Assert.Equal(addTaskDto.Important, task.Important);
        Assert.Equal(addTaskDto.Repeated, task.Repeated);

        //TODO : use value object to auto check properties
    }
}