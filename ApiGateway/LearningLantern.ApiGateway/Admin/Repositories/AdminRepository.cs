using LearningLantern.ApiGateway.Data.Models;
using LearningLantern.Common;
using Microsoft.AspNetCore.Identity;

namespace LearningLantern.ApiGateway.Admin.Repositories;

public class AdminRepository : IAdminRepository
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<UserModel> _userManager;

    public AdminRepository(UserManager<UserModel> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<IdentityResult> CreateAdminRoleAsync() =>
        await _roleManager.CreateAsync(new IdentityRole(LearningLanternRoles.Admin));

    public async Task<IdentityResult> AddToRoleAdminAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
            return IdentityResult.Failed(new IdentityError
            {
                Code = StatusCodes.Status404NotFound.ToString(),
                Description = $"userId: {userId} not found"
            });

        return await _userManager.AddToRoleAsync(user, LearningLanternRoles.Admin);
    }

    public async Task<IdentityResult> CreateUniversityAdminRoleAsync() =>
        await _roleManager.CreateAsync(new IdentityRole(LearningLanternRoles.UniversityAdmin));

    public async Task<IdentityResult> AddToRoleUniversityAdminAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
            return IdentityResult.Failed(new IdentityError
            {
                Code = StatusCodes.Status404NotFound.ToString(),
                Description = $"userId: {userId} not found"
            });

        return await _userManager.AddToRoleAsync(user, LearningLanternRoles.UniversityAdmin);
    }

    public async Task<IdentityResult> CreateInstructorRoleAsync() =>
        await _roleManager.CreateAsync(new IdentityRole(LearningLanternRoles.Instructor));
}