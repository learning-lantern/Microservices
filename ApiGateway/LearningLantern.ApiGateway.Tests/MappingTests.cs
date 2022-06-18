using System;
using LearningLantern.ApiGateway.Classroom.DTOs;
using LearningLantern.ApiGateway.Classroom.Models;
using LearningLantern.ApiGateway.Utility;
using LearningLantern.Common.Tests;
using Xunit;

namespace LearningLantern.ApiGateway.Tests;

public class MappingTests : MappingTestsSetup
{
    public MappingTests()
        : base(config => config.AddProfile<MappingProfile>())
    {
    }

    [Theory]
    [InlineData(typeof(ClassroomModel), typeof(ClassroomDTO))]
    [InlineData(typeof(ClassroomDTO), typeof(ClassroomModel))]
    [InlineData(typeof(ClassroomModel), typeof(AddClassroomDTO))]
    [InlineData(typeof(AddClassroomDTO), typeof(ClassroomModel))]
    public override void ShouldSupportMappingFromSourceToDestination(Type source, Type destination)
    {
        base.ShouldSupportMappingFromSourceToDestination(source, destination);
    }
}