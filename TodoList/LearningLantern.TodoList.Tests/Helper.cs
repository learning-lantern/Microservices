using System;
using AutoMapper;
using Bogus;
using LearningLantern.Common.Models.TodoModels;
using LearningLantern.TodoList.Utility;
using Microsoft.EntityFrameworkCore;

namespace LearningLantern.TodoList.Tests;

public static class Helper
{
    static Helper()
    {
        Randomizer.Seed = new Random(8675309);
        Faker.DefaultStrictMode = true;
    }

    public static UpdateTaskDTO GenerateUpdateTaskDTO()
    {
        var task = new Faker<UpdateTaskDTO>()
            .RuleFor(t => t.Title, f => f.Random.String(1, 10))
            .RuleFor(t => t.Note, f => f.Random.String(1, 10))
            .RuleFor(t => t.DueDate, f => DateTime.UtcNow.OrNull(f))
            .RuleFor(t => t.MyDay, f => f.Random.Bool())
            .RuleFor(t => t.Completed, f => f.Random.Bool())
            .RuleFor(t => t.Important, f => f.Random.Bool())
            .RuleFor(t => t.Repeated, f => f.Random.Number(1, 100));
        return task;
    }

    public static TaskDTO GenerateTaskDTO()
    {
        var task = new Faker<TaskDTO>()
            .RuleFor(t => t.Title, f => f.Random.String(1, 10))
            .RuleFor(t => t.Note, f => f.Random.String(1, 10))
            .RuleFor(t => t.DueDate, f => DateTime.UtcNow.OrNull(f))
            .RuleFor(t => t.MyDay, f => f.Random.Bool())
            .RuleFor(t => t.Completed, f => f.Random.Bool())
            .RuleFor(t => t.Important, f => f.Random.Bool())
            .RuleFor(t => t.Repeated, f => f.Random.Number(1, 10))
            .RuleFor(t => t.UserId, f => f.Random.String(1, 10));
        return task;
    }

    public static DbContextOptions UseInMemoryDatabaseOptions(string name)
    {
        var builder = new DbContextOptionsBuilder();
        builder.UseInMemoryDatabase(name);
        return builder.Options;
    }

    public static TodoContextMock CreateTodoContextMock(string name) => new(UseInMemoryDatabaseOptions(name));

    public static IMapper CreateAutoMapper()
    {
        var configuration = new MapperConfiguration(config =>
            config.AddProfile<MappingProfile>());

        return configuration.CreateMapper();
    }
}