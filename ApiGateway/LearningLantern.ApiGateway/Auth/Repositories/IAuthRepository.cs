using LearningLantern.ApiGateway.Auth.DTOs;
using Microsoft.AspNetCore.Identity;

namespace LearningLantern.ApiGateway.Auth.Repositories;

public interface IAuthRepository
{
    public Task<IdentityResult> CreateAsync(CreateDTO createDTO);

    public Task<SignInResponseDTO> SignInAsync(SignInDTO signInDTO);

    public Task<IdentityResult> SendConfirmationEmailAsync(string userEmail);

    public Task<IdentityResult> ConfirmEmailAsync(string userId, string token);
}