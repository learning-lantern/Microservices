using LearningLantern.ApiGateway.User.DTOs;

namespace LearningLantern.ApiGateway.Auth.DTOs;

/// <summary>
///     Sign in response data transfare object class.
/// </summary>
public class SignInResponseDTO
{
    public UserDTO? User;

    public SignInResponseDTO()
    {
    }

    public SignInResponseDTO(UserDTO userDTO, string token)
    {
        User = new UserDTO(userDTO);
        Token = token;
    }

    public string? Token { get; set; }
}