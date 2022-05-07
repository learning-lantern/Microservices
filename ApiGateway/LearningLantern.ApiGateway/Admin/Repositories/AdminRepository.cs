using LearningLantern.ApiGateway.Helpers;
using LearningLantern.ApiGateway.User.Models;
using Microsoft.AspNetCore.Identity;

namespace LearningLantern.ApiGateway.Admin.Repositories;

public class AdminRepository : IAdminRepository
{
    private readonly RoleManager<IdentityRole> roleManager;
    private readonly UserManager<UserModel> userManager;

    public AdminRepository(UserManager<UserModel> userManager, RoleManager<IdentityRole> roleManager)
    {
        this.userManager = userManager;
        this.roleManager = roleManager;
    }

    public async Task<IdentityResult> CreateAdminRoleAsync() => await roleManager.CreateAsync(new IdentityRole(Role.Admin));

    public async Task<IdentityResult> AddToRoleAdminAsync(string userId)
    {
        var user = await userManager.FindByIdAsync(userId);

        if (user == null)
            return IdentityResult.Failed(new IdentityError
            {
                Code = StatusCodes.Status404NotFound.ToString(),
                Description = Message.UserIdNotFound
            });

        return await userManager.AddToRoleAsync(user, Role.Admin);
    }

    public async Task<IdentityResult> CreateUniversityAdminRoleAsync() =>
        await roleManager.CreateAsync(new IdentityRole(Role.UniversityAdmin));

    public async Task<IdentityResult> AddToRoleUniversityAdminAsync(string userId)
    {
        var user = await userManager.FindByIdAsync(userId);

        if (user == null)
            return IdentityResult.Failed(new IdentityError
            {
                Code = StatusCodes.Status404NotFound.ToString(),
                Description = Message.UserIdNotFound
            });

        return await userManager.AddToRoleAsync(user, Role.UniversityAdmin);
    }

    public async Task<IdentityResult> CreateInstructorRoleAsync() =>
        await roleManager.CreateAsync(new IdentityRole(Role.Instructor));
}