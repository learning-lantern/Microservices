using LearningLantern.ApiGateway.Data.DTOs;
using LearningLantern.ApiGateway.Data.Models;
using LearningLantern.Common.Response;

namespace LearningLantern.ApiGateway.Repositories;

public interface IIdentityRepository
{
    public Task<Response> SignUpAsync(SignUpDTO signUpDTO);
    public Task<Response> SendConfirmationEmailAsync(string userEmail);
    public Task<Response<SignInResponseDTO>> SignInAsync(SignInDTO signInDTO);
    public Task<Response> ConfirmEmailAsync(string userId, string token);
    public Task<UserModel?> FindUserByIdAsync(string userId);
}