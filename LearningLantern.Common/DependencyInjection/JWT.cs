using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace LearningLantern.Common.DependencyInjection;

public static class JWT
{
    public const string ValidIssuer = "http://5001";
    public const string ValidAudience = "User";

    public static readonly SymmetricSecurityKey IssuerSigningKey =
        new(Encoding.UTF8.GetBytes("nDzMuunoP6JbalKjeyccXU3xUeMrQsVN"));
}