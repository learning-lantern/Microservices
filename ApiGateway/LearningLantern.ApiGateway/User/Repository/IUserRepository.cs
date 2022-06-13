using LearningLantern.ApiGateway.User.DTOs;
using LearningLantern.ApiGateway.User.Models;
using LearningLantern.Common.Response;

namespace LearningLantern.ApiGateway.User.Repository;

public interface IUserRepository
{
    public Task<Response> Signup(SignupDTO signupDTO);
    public Task<Response<SignInResponseDTO>> Login(LoginDTO loginDTO);


    public Task<Response> UpdateName(string userId, UpdateNameDTO updateNameDTO);
    public Task<Response> UpdatePassword(string userId, UpdatePasswordDTO updatePasswordDTO);
    public Task<Response> UpdateEmail(string userId, UpdateEmailDTO updateEmailDTO, string? endPointUrl);

    public Task<Response> SendConfirmationEmail(string userEmail, string? endPointUrl);
    public Task<Response> ConfirmEmail(string userId, string token);
    public Task<Response> ConfirmUpdateEmail(string userId, string newEmail, string token);


    public Task<Response> DeleteUser(string userId, DeleteUserDTO deleteUserDTO);


    public Task<Response<IEnumerable<UserDTO>>> GetAllUsers(int pageNumber, int pageSize);
    public Task<Response<UserDTO>> GetById(string userId);
    public Task<UserModel?> FindUserByIdAsync(string userId);
}