using LearningLantern.ApiGateway.Data;
using LearningLantern.ApiGateway.Data.Models;
using LearningLantern.ApiGateway.Events;
using LearningLantern.ApiGateway.Repositories;
using LearningLantern.ApiGateway.Services;
using LearningLantern.ApiGateway.Utility;
using LearningLantern.Common.DependencyInjection;
using LearningLantern.Common.EventBus;
using LearningLantern.Common.EventBus.EventProcessor;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
        services.AddAutoMapper(typeof(MappingProfile));
        services.AddInfrastructure();
        services.AddCorsForAngular();
        return services;
    }

    public static IApplicationBuilder AddSubscriptionsConfiguration(this IApplicationBuilder app)
    {
        var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
        //eventBus.Subscribe<UpdateEvent>("Update");
        return app;
    }

    private static IServiceCollection AddDatabaseConfiguration(this IServiceCollection services)
    {
        services.AddDbContext<ILearningLanternContext, LearningLanternContext>(builder =>
        {
            var myServerAddress = "learning-lantern.database.windows.net";
            var myUsername = "LearningLanternAdmin";
            var password = "TwajbuxAReMej9";
            var myDatabase = "ApiGateway";
            var connectionString =
                $"Server={myServerAddress};Database={myDatabase};User Id={myUsername};Password={password}";
            builder.UseSqlServer(connectionString);
        }).AddIdentity<UserModel, IdentityRole>(setupAction =>
        {
            setupAction.SignIn.RequireConfirmedAccount = true;
            setupAction.User.RequireUniqueEmail = true;
        }).AddEntityFrameworkStores<LearningLanternContext>().AddDefaultTokenProviders();
        return services;
    }

    private static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<ICurrentUserService, CurrentUserService>();
        services.AddHttpContextAccessor();

        services.AddSingleton<IEmailSender, EmailSender>(
            op => new EmailSender(ConfigProvider.MyEmail, ConfigProvider.MyPassword, ConfigProvider.SmtpServerAddress,
                ConfigProvider.MailPort)
        );
        services.AddSingleton<IEventProcessor, EventProcessor>();
        services.AddTransient<IIdentityRepository, IdentityRepository>();
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