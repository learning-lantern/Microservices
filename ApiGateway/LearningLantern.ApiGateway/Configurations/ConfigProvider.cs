using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace LearningLantern.ApiGateway.Configurations;

public static class ConfigProvider
{
    public static IConfiguration Configuration = null!;

    public static readonly string AuthenticationProviderKey = "JWTAuthentication";
    public static string JWTValidAudience => Get("JWT:ValidAudience");
    public static string JWTValidIssuer => Get("JWT:ValidIssuer");
    public static string JWTSecret => Get("JWT:Secret");
    public static SymmetricSecurityKey JWTIssuerSigningKey => new(Encoding.UTF8.GetBytes(JWTSecret));


    public static string MyEmail => Get("Email:Email", "");
    public static string MyPassword => Get("Email:Password", "");
    public static string SmtpServerAddress => Get("Email:SMTPServerAddress", "smtp.gmail.com");
    public static int MailPort => Convert.ToInt32(Get("Email:MailPort", "587"));

    private static string Get(string name) =>
        Configuration[name] ?? throw new Exception("Configuration for " + name + " not found");

    private static string Get(string name, string defaultValue) => Configuration[name] ?? defaultValue;
}