using FluentAssertions;
using LearningLantern.ApiGateway.Data.DTOs;
using LearningLantern.ApiGateway.Tests.Helpers;
using Xunit;

namespace LearningLantern.ApiGateway.Tests.Repository.ClassroomRepositoryTests;

public class ClassroomRepositoryAddAsync : ClassroomRepositoryTestSetup
{
    private readonly AddClassroomDTO _randomAddClassroomDTO;

    public ClassroomRepositoryAddAsync()
    {
        _randomAddClassroomDTO = Helper.GenerateRandomAddClassroomDTO();
    }

    [Fact]
    public async void ShouldCallSaveChangesOnlyOnce()
    {
        //arrange
        //act
        await ClassroomRepository.AddAsync(_randomAddClassroomDTO);
        //assert
        Assert.Equal(1, Context.CountCalls);
    }

    [Fact]
    public async void TestAddClassroom()
    {
        //arrange
        //act
        var response = await ClassroomRepository.AddAsync(_randomAddClassroomDTO);
        //assert
        Assert.True(response.Succeeded);
        response.Data.Should().BeEquivalentTo(_randomAddClassroomDTO,
            options => options.ExcludingMissingMembers());
        Assert.Equal(1, Context.CountCalls);
    }
}