using LearningLantern.ApiGateway.Helpers;
using LearningLantern.ApiGateway.User.DTOs;
using LearningLantern.ApiGateway.User.Models;
using Microsoft.AspNetCore.Identity;

namespace LearningLantern.ApiGateway.User.Repositories;

public class UserRepository : IUserRepository
{
    private readonly UserManager<UserModel> userManager;

    public UserRepository(UserManager<UserModel> userManager)
    {
        this.userManager = userManager;
    }

    public async Task<UserDTO?> FindByIdAsync(string userId)
    {
        var user = await userManager.FindByIdAsync(userId);

        return user == null || !user.EmailConfirmed ? null : new UserDTO(user);
    }

    public async Task<UserDTO?> FindByEmailAsync(string userEmail)
    {
        var user = await userManager.FindByEmailAsync(userEmail);

        return user == null || !user.EmailConfirmed ? null : new UserDTO(user);
    }

    public async Task<UserDTO?> UpdateAsync(UserDTO userDTO)
    {
        var user = await userManager.FindByIdAsync(userDTO.Id);

        if (user == null) return null;

        user.FirstName = userDTO.FirstName.Trim();
        user.LastName = userDTO.LastName.Trim();

        var updateAsyncResult = await userManager.UpdateAsync(user);

        return updateAsyncResult.Succeeded ? new UserDTO(user) : null;
    }

    public async Task<IdentityResult> DeleteAsync(string userEmail, string userPassword)
    {
        var user = await userManager.FindByEmailAsync(userEmail);

        if (user == null)
            return IdentityResult.Failed(new IdentityError
            {
                Code = StatusCodes.Status404NotFound.ToString(),
                Description = Message.UserEmailNotFound
            });

        return await userManager.DeleteAsync(user);
    }

    public async Task<UserModel?> FindUserByIdAsync(string userId) => await userManager.FindByIdAsync(userId);
}