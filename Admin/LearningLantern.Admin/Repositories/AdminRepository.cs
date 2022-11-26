using LearningLantern.ApiGateway.Data.Models;
using LearningLantern.Common;
using Microsoft.AspNetCore.Identity;

namespace LearningLantern.Admin.Repositories;

public class AdminRepository : IAdminRepository
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<UserModel> _userManager;

    public AdminRepository(UserManager<UserModel> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<IdentityResult> AddToRoleInstructorAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
            return IdentityResult.Failed(new IdentityError
            {
                Code = StatusCodes.Status404NotFound.ToString(),
                Description = $"userId: {userId} not found"
            });

        return await _userManager.AddToRoleAsync(user, LearningLanternRoles.Instructor);
    }

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

    public async Task<IdentityResult> CreateRolesAsync()
    {
        foreach (var x in LearningLanternRoles.AllRoles)
        {
            var identityResult = await _roleManager.CreateAsync(new IdentityRole(x));
            if (identityResult.Succeeded == false) return identityResult;
        }

        return IdentityResult.Success;
    }
}