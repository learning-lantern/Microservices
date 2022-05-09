using System;
using AutoMapper;
using Bogus;
using LearningLantern.TodoList.Data.Models;
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

    public static string GenerateRandomUserId()
    {
        return Guid.NewGuid().ToString();
    }
    
    public static TaskProperties GenerateTaskProperties()
    {
        var task = new Faker<TaskProperties>()
            .RuleFor(t => t.Title, f => f.Random.String(1, 10))
            .RuleFor(t => t.Note, f => f.Random.String(1, 10))
            .RuleFor(t => t.DueDate, f => DateTime.UtcNow.OrNull(f))
            .RuleFor(t => t.MyDay, f => f.Random.Bool())
            .RuleFor(t => t.Completed, f => f.Random.Bool())
            .RuleFor(t => t.Important, f => f.Random.Bool())
            .RuleFor(t => t.Repeated, f => f.Random.Number(1, 100));
        return task;
    }


    public static AddTaskDTO GenerateAddTaskDTO()
    {
        var task = new Faker<AddTaskDTO>()
            .RuleFor(t => t.Title, f => f.Random.String(1, 10))
            .RuleFor(t => t.Note, f => f.Random.String(1, 10))
            .RuleFor(t => t.DueDate, f => DateTime.UtcNow.OrNull(f))
            .RuleFor(t => t.MyDay, f => f.Random.Bool())
            .RuleFor(t => t.Completed, f => f.Random.Bool())
            .RuleFor(t => t.Important, f => f.Random.Bool())
            .RuleFor(t => t.Repeated, f => f.Random.Number(1, 100))
            .RuleFor(t => t.TempId, f => f.Random.String(1, 10));
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