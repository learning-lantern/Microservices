using FluentAssertions;
using LearningLantern.ApiGateway.Classroom.DTOs;
using LearningLantern.ApiGateway.Tests.Helpers;
using Xunit;

namespace LearningLantern.ApiGateway.Tests.Repository.ClassroomRepositoryTests;

public class ClassroomRepositoryUpdateAsync : ClassroomRepositoryTestSetup
{
    private readonly ClassroomDTO _randomClassroomDTO;

    public ClassroomRepositoryUpdateAsync()
    {
        var randomClassroom = Helper.GenerateRandomAddClassroomDTO();
        var response = ClassroomRepository.AddAsync(randomClassroom).GetAwaiter().GetResult();
        var classroomDTO = response.Data!;
        _randomClassroomDTO = Helper.GenerateRandomClassroomDTO();
        _randomClassroomDTO.Id = classroomDTO.Id;
        Context.CountCalls = 0;
    }

    [Fact]
    public async void ShouldCallSaveChangesOnlyOnce()
    {
        //arrange
        //act
        await ClassroomRepository.UpdateAsync(_randomClassroomDTO);
        //assert
        Assert.Equal(1, Context.CountCalls);
    }

    [Fact]
    public async void TestUpdateClassroom()
    {
        //arrange
        //act
        var response = await ClassroomRepository.UpdateAsync(_randomClassroomDTO);
        //assert
        Assert.Equal(1, Context.CountCalls);
        Assert.True(response.Succeeded);
        ClassroomRepository.GetClassroomById(_randomClassroomDTO.Id).Should().BeEquivalentTo(_randomClassroomDTO,
            options => options.ExcludingMissingMembers());
    }
}