using Microsoft.AspNetCore.Identity;

namespace LearningLantern.ApiGateway.University.Repositories;

public interface IUniversityRepository
{
    public Task<IdentityResult> AddToRoleInstructorAsync(string userId);
}