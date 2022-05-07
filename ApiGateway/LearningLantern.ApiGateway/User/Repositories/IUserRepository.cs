using LearningLantern.ApiGateway.User.DTOs;
using LearningLantern.ApiGateway.User.Models;
using Microsoft.AspNetCore.Identity;

namespace LearningLantern.ApiGateway.User.Repositories;

public interface IUserRepository
{
    public Task<UserDTO?> FindByIdAsync(string userId);

    public Task<UserDTO?> FindByEmailAsync(string userEmail);

    public Task<UserDTO?> UpdateAsync(UserDTO userDTO);

    public Task<IdentityResult> DeleteAsync(string userEmail, string userPassword);

    public Task<UserModel?> FindUserByIdAsync(string userId);
}