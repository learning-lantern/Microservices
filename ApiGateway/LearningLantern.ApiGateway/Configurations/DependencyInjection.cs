using LearningLantern.ApiGateway.Admin.Repositories;
using LearningLantern.ApiGateway.Auth.Repositories;
using LearningLantern.ApiGateway.Classroom.Repositories;
using LearningLantern.ApiGateway.Database;
using LearningLantern.ApiGateway.Helpers;
using LearningLantern.ApiGateway.University.Repositories;
using LearningLantern.ApiGateway.User.Models;
using LearningLantern.ApiGateway.User.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MMLib.SwaggerForOcelot.DependencyInjection;
using Ocelot.DependencyInjection;
using Ocelot.Provider.Polly;

namespace LearningLantern.ApiGateway.Configurations;

public static class DependencyInjection
{
    public static WebApplicationBuilder AddOcelotConfiguration(this WebApplicationBuilder builder)
    {
        // https://mahedee.net/configure-swagger-on-api-gateway-using-ocelot-in-asp.net-core-application/
        const string routes = "Ocelot";
        builder.Configuration.AddOcelotWithSwaggerSupport(options => { options.Folder = routes; });
        builder.Services.AddOcelot(builder.Configuration).AddPolly();
        builder.Services.AddSwaggerForOcelot(builder.Configuration);
        builder.Configuration.SetBasePath(Directory.GetCurrentDirectory())
            .AddOcelot(routes, builder.Environment)
            .AddEnvironmentVariables();
        return builder;
    }

    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddDatabaseConfiguration();
        services.AddAuthenticationConfigurations();
        services.AddInfrastructure();
        services.AddCorsForAngular();
        return services;
    }

    private static IServiceCollection AddDatabaseConfiguration(this IServiceCollection services)
    {
        services.AddDbContext<LearningLanternContext>(builder =>
        {
            var myServerAddress = "learning-lantern.database.windows.net";
            var myDatabase = "ApiGateway";
            var myUsername = "learinglanternadmin";
            var password = "z8ZkHzdWw8a79B";
            var connectionString =
                $"Server={myServerAddress};Database={myDatabase};User Id={myUsername};Password={password}";
            builder.UseSqlServer(connectionString);
        });
        return services;
    }

    private static IServiceCollection AddAuthenticationConfigurations(this IServiceCollection services)
    {
        
        services.AddIdentity<UserModel, IdentityRole>(setupAction =>
        {
            setupAction.SignIn.RequireConfirmedAccount = true;
            setupAction.User.RequireUniqueEmail = true;
        }).AddEntityFrameworkStores<LearningLanternContext>().AddDefaultTokenProviders();
        services.AddAuthentication(configureOptions =>
        {
            configureOptions.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            configureOptions.DefaultForbidScheme = JwtBearerDefaults.AuthenticationScheme;
            configureOptions.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
            configureOptions.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
            configureOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            configureOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(ConfigProvider.AuthenticationProviderKey, configureOptions =>
        {
            configureOptions.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = ConfigProvider.JWTValidIssuer,
                ValidateAudience = true,
                ValidAudience = ConfigProvider.JWTValidAudience,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = ConfigProvider.JWTIssuerSigningKey
            };
        });
        return services;
    }


    private static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<ICurrentUserService, CurrentUserService>();
        services.AddHttpContextAccessor();
        
        // TODO: Rethink about each service life time (Singleton, Scoped, or Transient).
        services.AddTransient<IAuthRepository, AuthRepository>();
        services.AddTransient<IUserRepository, UserRepository>();
        services.AddTransient<IAdminRepository, AdminRepository>();
        services.AddTransient<IUniversityRepository, UniversityRepository>();
        services.AddTransient<IClassroomRepository, ClassroomRepository>();
        return services;
    }

    private static IServiceCollection AddCorsForAngular(this IServiceCollection services)
    {
        services.AddCors(setupAction => setupAction.AddDefaultPolicy(
            policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));
        return services;
    }
}