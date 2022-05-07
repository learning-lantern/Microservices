using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace LearningLantern.ApiGateway.Helpers;

public static class JWT
{
    public const string ValidIssuer = "ApiGateway";
    public const string ValidAudience = "ApiGateway";

    public static readonly SymmetricSecurityKey IssuerSigningKey =
        new(Encoding.UTF8.GetBytes("nDzMuunoP6JbalKjeyccXU3xUeMrQsVN"));
}