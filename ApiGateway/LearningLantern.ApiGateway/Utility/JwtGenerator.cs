using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using LearningLantern.ApiGateway.Data.DTOs;
using LearningLantern.ApiGateway.Data.Models;
using LearningLantern.Common;
using LearningLantern.Common.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace LearningLantern.ApiGateway.Utility;

public class UserTokenData
{
    public UserTokenData(string firstName, string lastName, bool emailConfirmed)
    {
        FirstName = firstName;
        LastName = lastName;
        EmailConfirmed = emailConfirmed;
    }

    public string FirstName { get; }
    public string LastName { get; }
    public bool EmailConfirmed { get; }
}

public class JWTGenerator
{
    private readonly UserManager<UserModel> _userManager;

    public JWTGenerator(UserManager<UserModel> userManager)
    {
        _userManager = userManager;
    }

    public async Task<TokenResponseDTO> GenerateJwtSecurityToken(UserModel user)
    {
        var roles = await _userManager.GetRolesAsync(user);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(nameof(user.FirstName), user.FirstName),
            new(nameof(user.LastName), user.LastName),
            new(nameof(user.EmailConfirmed), user.EmailConfirmed.ToString())
        };
        if (roles.Any())
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
        else
            claims.Add(new Claim(ClaimTypes.Role, LearningLanternRoles.Student));

        var token = new JwtSecurityTokenHandler().WriteToken(new JwtSecurityToken(
            JWT.ValidIssuer,
            JWT.ValidAudience,
            claims,
            expires: DateTime.UtcNow.AddDays(30),
            signingCredentials: new SigningCredentials(JWT.IssuerSigningKey,
                SecurityAlgorithms.HmacSha256Signature)
        ));
        return new TokenResponseDTO(token);
    }
}