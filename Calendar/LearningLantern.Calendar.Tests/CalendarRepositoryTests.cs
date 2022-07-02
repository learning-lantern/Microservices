using AutoMapper;
using LearningLantern.Calendar.Exceptions;
using LearningLantern.Calendar.Repositories;
using Xunit;

namespace LearningLantern.Calendar.Tests;

public class CalendarRepositoryTests
{
    private readonly IMapper _mapper;

    public CalendarRepositoryTests()
    {
        _mapper = Helper.CreateAutoMapper();
    }

    [Fact]
    public async void ShouldCallSaveChangesOnlyOne_AddAsync()
    {
        //arrange
        await using var context = Helper.CreateCalendarContextMock(
            nameof(ShouldCallSaveChangesOnlyOne_AddAsync));
        var calendarRepository = new CalendarRepository(context, _mapper);
        //act
        var response = await calendarRepository.AddAsync(Helper.GenerateEventDTO());
        //assert
        Assert.True(response.Succeeded);
        Assert.Equal(1, context.CountCalls);
    }

    [Fact]
    public async void ShouldCallSaveChangesOnlyOne_UpdateAsync()
    {
        //arrange
        await using var context = Helper.CreateCalendarContextMock(
            nameof(ShouldCallSaveChangesOnlyOne_UpdateAsync));
        var calendarRepository = new CalendarRepository(context, _mapper);
        var response = await calendarRepository.AddAsync(Helper.GenerateEventDTO());
        context.CountCalls = 0;
        //act
        await calendarRepository.UpdateAsync(response.Data.Id, response.Data);
        //assert
        Assert.Equal(1, context.CountCalls);
    }

    [Fact]
    public async void ShouldCallSaveChangesOnlyOne_RemoveAsync()
    {
        //arrange
        await using var context = Helper.CreateCalendarContextMock(
            nameof(ShouldCallSaveChangesOnlyOne_RemoveAsync));
        var calendarRepository = new CalendarRepository(context, _mapper);
        var id = (await calendarRepository.AddAsync(Helper.GenerateEventDTO())).Data.Id;
        context.CountCalls = 0;
        //act
        await calendarRepository.RemoveAsync(id);
        //assert
        Assert.Equal(1, context.CountCalls);
    }

    [Fact]
    public async void ShouldCallSaveChangesOnlyZeroWhenNotFound_RemoveAsync()
    {
        //arrange
        await using var context = Helper.CreateCalendarContextMock(
            nameof(ShouldCallSaveChangesOnlyZeroWhenNotFound_RemoveAsync));
        var todoRepository = new CalendarRepository(context, _mapper);
        //act
        await todoRepository.RemoveAsync(123);
        //assert
        Assert.Equal(0, context.CountCalls);
    }

    [Fact]
    public async void TestAddEvent()
    {
        //arrange
        await using var context = Helper.CreateCalendarContextMock(
            nameof(TestAddEvent));
        var calendarRepository = new CalendarRepository(context, _mapper);
        //act
        var response = await calendarRepository.AddAsync(Helper.GenerateEventDTO());
        //assert
        Assert.True(response.Succeeded);
        var eventModel = Assert.Single(context.Events);
        Assert.Equal(eventModel.Id, response.Data.Id);
    }
}