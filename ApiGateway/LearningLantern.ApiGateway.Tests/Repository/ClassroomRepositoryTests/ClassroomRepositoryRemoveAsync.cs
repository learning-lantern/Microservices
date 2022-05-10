using System.Threading.Tasks;
using LearningLantern.ApiGateway.Tests.Helpers;
using LearningLantern.ApiGateway.Utility;
using Xunit;

namespace LearningLantern.ApiGateway.Tests.Repository.ClassroomRepositoryTests;

public class ClassroomRepositoryRemoveAsync : ClassroomRepositoryTestSetup
{
    [Fact]
    public async void ShouldCallSaveChangesOnlyOnceWhenFound()
    {
        //arrange
        var id = await AddClassroomToRepository();
        //act
        await ClassroomRepository.RemoveAsync(id);
        //assert
        Assert.Equal(1, Context.CountCalls);
    }

    [Fact]
    public async void TestRemoveClassroomWhenFound()
    {
        //arrange
        var id = await AddClassroomToRepository();
        //act
        var response = await ClassroomRepository.RemoveAsync(id);
        //assert
        Assert.True(response.Succeeded);
        Assert.Empty(Context.Classrooms);
    }

    [Fact]
    public async void ShouldCallSaveChangesZeroTimesWhenNotFound()
    {
        //arrange
        //act
        await ClassroomRepository.RemoveAsync(123);
        //assert
        Assert.Equal(0, Context.CountCalls);
    }

    [Fact]
    public async void TestRemoveClassroomWhenNotFound()
    {
        //arrange
        //act
        var response = await ClassroomRepository.RemoveAsync(123);
        //assert
        Assert.False(response.Succeeded);
        Assert.NotNull(response.Errors);
        Assert.Contains(response.Errors!, e => e.ErrorCode == nameof(ErrorsList.ClassroomIdNotFound));
    }

    async private Task<int> AddClassroomToRepository()
    {
        var randomClassroom = Helper.GenerateRandomAddClassroomDTO();
        var response = await ClassroomRepository.AddAsync(randomClassroom);
        Context.CountCalls = 0;
        return response.Data!.Id;
    }
}