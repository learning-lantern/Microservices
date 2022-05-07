using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace LearningLantern.Common.DependencyInjection;

public static class DependencyInjection
{
    public static void AddAuthorizedSwaggerGen(this IServiceCollection services, string title, string version)
    {
        services.AddSwaggerGen(option =>
        {
            option.SwaggerDoc(version, new OpenApiInfo {Title = title, Version = version});
            option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description =
                    "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });
            option.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
                }
            });
        });
    }
}