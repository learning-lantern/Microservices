using System;
using LearningLantern.Common.Tests;
using LearningLantern.TodoList.Data.Models;
using LearningLantern.TodoList.Utility;
using Xunit;

namespace LearningLantern.TodoList.Tests;

public class MappingTests : MappingTestsSetup
{
    public MappingTests()
        : base(config => config.AddProfile<MappingProfile>())
    {
    }

    [Theory]
    [InlineData(typeof(TaskProperties), typeof(TaskModel))]
    [InlineData(typeof(TaskModel), typeof(TaskDTO))]
    public override void ShouldSupportMappingFromSourceToDestination(Type source, Type destination)
    {
        base.ShouldSupportMappingFromSourceToDestination(source, destination);
    }
}