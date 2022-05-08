using System;
using AutoMapper;
using Bogus;
using LearningLantern.Calendar.Data.Models;
using LearningLantern.Calendar.Utility;
using Microsoft.EntityFrameworkCore;

namespace LearningLantern.Calendar.Tests;

public static class Helper
{
    static Helper()
    {
        Randomizer.Seed = new Random(8675309);
        Faker.DefaultStrictMode = true;
    }

    public static UpdateEventDTO GenerateUpdateEventDTO()
    {
        var eventProperties = new Faker<UpdateEventDTO>()
            .RuleFor(t => t.Title, f => f.Random.String(1, 1000))
            .RuleFor(t => t.Description, f => f.Random.String(1, 1000))
            .RuleFor(t => t.StartTime, f => DateTime.UtcNow)
            .RuleFor(t => t.EndTime, f => DateTime.UtcNow);

        return eventProperties;
    }

    public static EventDTO GenerateEventDTO()
    {
        var eventDTO = new Faker<EventDTO>()
            .RuleFor(t => t.Title, f => f.Random.String(1, 1000))
            .RuleFor(t => t.Description, f => f.Random.String(1, 1000))
            .RuleFor(t => t.StartTime, f => DateTime.UtcNow)
            .RuleFor(t => t.EndTime, f => DateTime.UtcNow)
            .RuleFor(t => t.ClassroomId, f => f.Random.Int(1, 100));

        return eventDTO;
    }

    public static IMapper CreateAutoMapper()
    {
        var configuration = new MapperConfiguration(config =>
            config.AddProfile<MappingProfile>());

        return configuration.CreateMapper();
    }

    public static DbContextOptions UseInMemoryDatabaseOptions(string name)
    {
        var builder = new DbContextOptionsBuilder();
        builder.UseInMemoryDatabase(name);
        return builder.Options;
    }

    public static CalendarContextMock CreateCalendarContextMock(string name) => new(UseInMemoryDatabaseOptions(name));
}