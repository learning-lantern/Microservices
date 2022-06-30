namespace LearningLantern.ApiGateway.Data.DTOs;

/// <summary>
///     Sign in response data transfer object class.
/// </summary>
public class TokenResponseDTO
{
    public TokenResponseDTO(string token)
    {
        Token = token;
    }

    public string Token { get; }
}