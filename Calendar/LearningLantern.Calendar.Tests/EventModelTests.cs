using LearningLantern.Calendar.Data.Models;
using Xunit;

namespace LearningLantern.Calendar.Tests;

public class EventModelTests
{
    [Fact]
    public void TestUpdateFunction()
    {
        // arrange
        var eventModel = new EventModel();
        var eventProperties = Helper.GenerateUpdateEventDTO();
        // act
        eventModel.Update(eventProperties);
        // assert
        Assert.Equal(eventModel.Title, eventProperties.Title);
        Assert.Equal(eventModel.Description, eventProperties.Description);
        Assert.Equal(eventModel.StartDate, eventProperties.StartDate);
        Assert.Equal(eventModel.DueDate, eventProperties.DueDate);
    }
}