using LearningLantern.ApiGateway.Data.DTOs;
using LearningLantern.ApiGateway.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace LearningLantern.ApiGateway.Repositories;

public interface IIdentityRepository
{
    public Task<IdentityResult> SignUpAsync(SignUpDTO signUpDTO);
    public Task<SignInResponseDTO> SignInAsync(SignInDTO signInDTO);
    public Task<IdentityResult> SendConfirmationEmailAsync(string userEmail);
    public Task<IdentityResult> ConfirmEmailAsync(string userId, string token);
    public Task<UserDTO?> FindByIdAsync(string userId);
    public Task<UserModel?> FindUserByIdAsync(string userId);
    public Task<UserDTO> FindByEmailAsync(string userEmail);
    public Task<UserDTO> UpdateAsync(UserDTO userDTO);
    public Task<IdentityResult> DeleteAsync(string userEmail, string userPassword);
}