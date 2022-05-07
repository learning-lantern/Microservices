using LearningLantern.ApiGateway.Helpers;
using LearningLantern.ApiGateway.User.Models;
using Microsoft.AspNetCore.Identity;

namespace LearningLantern.ApiGateway.University.Repositories;

public class UniversityRepository : IUniversityRepository
{
    private readonly UserManager<UserModel> _userManager;

    public UniversityRepository(UserManager<UserModel> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IdentityResult> AddToRoleInstructorAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
            return IdentityResult.Failed(new IdentityError
            {
                Code = StatusCodes.Status404NotFound.ToString(),
                Description = Message.UserIdNotFound
            });

        return await _userManager.AddToRoleAsync(user, Role.Instructor);
    }
}