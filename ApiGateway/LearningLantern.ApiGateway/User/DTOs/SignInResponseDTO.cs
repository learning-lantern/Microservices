namespace LearningLantern.ApiGateway.User.DTOs;

/// <summary>
///     Sign in response data transfer object class.
/// </summary>
public class SignInResponseDTO
{
    public SignInResponseDTO(UserDTO userDTO, string token)
    {
        User = userDTO;
        Token = token;
    }

    public UserDTO User { get; set; } = null!;

    public string Token { get; set; } = null!;
}