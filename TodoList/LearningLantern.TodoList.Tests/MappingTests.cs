using System;
using System.Runtime.Serialization;
using AutoMapper;
using LearningLantern.Common.Models.TodoModels;
using LearningLantern.TodoList.Utility;
using Xunit;

namespace LearningLantern.TodoList.Tests;

public class MappingTests
{
    private readonly IConfigurationProvider _configuration;
    private readonly IMapper _mapper;

    public MappingTests()
    {
        _configuration = new MapperConfiguration(config =>
            config.AddProfile<MappingProfile>());

        _mapper = _configuration.CreateMapper();
    }

    [Fact]
    public void ShouldHaveValidConfiguration()
    {
        _configuration.AssertConfigurationIsValid();
    }


    [Theory]
    [InlineData(typeof(TaskDTO), typeof(TaskModel))]
    [InlineData(typeof(TaskModel), typeof(TaskDTO))]
    public void ShouldSupportMappingFromSourceToDestination(Type source, Type destination)
    {
        var instance = GetInstanceOf(source);

        _mapper.Map(instance, source, destination);
    }

    private object GetInstanceOf(Type type)
    {
        if (type.GetConstructor(Type.EmptyTypes) != null)
            return Activator.CreateInstance(type)!;
        // Type without parameterless constructor
        return FormatterServices.GetUninitializedObject(type);
    }
}