using LearningLantern.Common.EventBus;
using LearningLantern.Common.EventBus.RabbitMQConnection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RabbitMQ.Client;

namespace LearningLantern.Common.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddAuthenticationConfigurations(this IServiceCollection services)
    {
        services.AddAuthentication(configureOptions =>
        {
            configureOptions.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            configureOptions.DefaultForbidScheme = JwtBearerDefaults.AuthenticationScheme;
            configureOptions.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
            configureOptions.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
            configureOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            configureOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(configureOptions =>
        {
            configureOptions.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = JWT.ValidIssuer,
                ValidateAudience = true,
                ValidAudience = JWT.ValidAudience,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = JWT.IssuerSigningKey
            };
        });
        return services;
    }

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
                Type = SecuritySchemeType.Http,
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
                    Array.Empty<string>()
                }
            });
        });
    }

    public static IServiceCollection AddRabbitMQ(this IServiceCollection services, IConnectionFactory connectionFactory)
    {
        return services.AddSingleton<IRabbitMQConnection, RabbitMQConnection>(
            op => new RabbitMQConnection(connectionFactory)
        ).AddSingleton<IEventBus, RabbitMQBus>();
    }
}