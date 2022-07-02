using System;
using AutoFixture;
using AutoMapper;
using Bogus;
using LearningLantern.TodoList.Data.Models;
using LearningLantern.TodoList.Utility;
using Microsoft.EntityFrameworkCore;

namespace LearningLantern.TodoList.Tests.Helpers;

public static class Helper
{
    private static readonly Fixture _fixture = new();

    static Helper()
    {
        Randomizer.Seed = new Random(8675309);
        Faker.DefaultStrictMode = true;
    }

    public static string GenerateRandomUserId() => Guid.NewGuid().ToString();

    public static TaskProperties GenerateTaskProperties()
    {
        var task = _fixture.Create<TaskProperties>();
        return task;
    }

    public static TodoContextMock CreateTodoContextMock(string name)
    {
        var builder = new DbContextOptionsBuilder();
        builder.UseInMemoryDatabase(name);
        return new TodoContextMock(builder.Options);
    }

    public static IMapper CreateAutoMapper()
    {
        var configuration = new MapperConfiguration(config =>
            config.AddProfile<MappingProfile>());
        configuration.AssertConfigurationIsValid();
        return configuration.CreateMapper();
    }
}