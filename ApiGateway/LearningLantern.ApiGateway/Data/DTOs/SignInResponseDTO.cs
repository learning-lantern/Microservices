namespace LearningLantern.ApiGateway.Data.DTOs;

/// <summary>
///     Sign in response data transfer object class.
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