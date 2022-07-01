using System;
using AutoMapper;
using Bogus;
using LearningLantern.ApiGateway.Data.DTOs;
using LearningLantern.ApiGateway.Utility;
using Microsoft.EntityFrameworkCore;

namespace LearningLantern.ApiGateway.ClassroomTests.Helpers;

public static class Helper
{
    static Helper()
    {
        Randomizer.Seed = new Random(8675309);
        Faker.DefaultStrictMode = true;
    }

    public static LearningLanternContextMock LearningLanternContextMock(string name)
    {
        var builder = new DbContextOptionsBuilder();
        builder.UseInMemoryDatabase(name);
        return new LearningLanternContextMock(builder.Options);
    } 

    public static IMapper CreateAutoMapper()
    {
        var configuration = new MapperConfiguration(config =>
            config.AddProfile<MappingProfile>());
        //configuration.AssertConfigurationIsValid();
        return configuration.CreateMapper();
    }


    public static string GenerateRandomUserId() => Guid.NewGuid().ToString();

    public static AddClassroomDTO GenerateRandomAddClassroomDTO()
    {
        return new Faker<AddClassroomDTO>()
            .RuleFor(t => t.Name, f => f.Random.String())
            .RuleFor(t => t.Description, f => f.Random.String());
    }

    public static ClassroomDTO GenerateRandomClassroomDTO()
    {
        return new Faker<ClassroomDTO>()
            .RuleFor(t => t.Id, f => f.Random.Int())
            .RuleFor(t => t.Name, f => f.Random.String())
            .RuleFor(t => t.Description, f => f.Random.String());
    }
}