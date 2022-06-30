using System.Reflection;
using FluentValidation.AspNetCore;
using LearningLantern.ApiGateway.Classroom.Repositories;
using LearningLantern.ApiGateway.Data;
using LearningLantern.ApiGateway.Data.Models;
using LearningLantern.ApiGateway.PipelineBehaviors;
using LearningLantern.ApiGateway.Users.Events;
using LearningLantern.ApiGateway.Utility;
using LearningLantern.Common.DependencyInjection;
using LearningLantern.Common.EventBus;
using LearningLantern.Common.EventBus.EventProcessor;
using LearningLantern.Common.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MMLib.SwaggerForOcelot.DependencyInjection;
using Ocelot.DependencyInjection;
using Ocelot.Provider.Polly;
using RabbitMQ.Client;

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

    public static IServiceCollection AddApplicationConfiguration(this IServiceCollection services)
    {
        services.AddDatabaseConfiguration();
        services.AddAuthenticationConfigurations();
        services.AddRabbitMQConfiguration();

        services.AddAutoMapper(typeof(MappingProfile));
        services.AddInfrastructure();

        services.AddValidations();
        services.AddMediatRConfiguration();

        services.AddCorsForAngular();
        return services;
    }

    private static IServiceCollection AddDatabaseConfiguration(this IServiceCollection services)
    {
        services.AddDbContext<ILearningLanternContext, LearningLanternContext>(builder =>
        {
            builder.UseSqlServer(ConfigProvider.DefaultConnectionString);
        }).AddIdentity<UserModel, IdentityRole>(setupAction =>
        {
            setupAction.SignIn.RequireConfirmedAccount = false;
            setupAction.User.RequireUniqueEmail = true;
            setupAction.Password.RequireNonAlphanumeric = false;
            setupAction.Password.RequiredUniqueChars = 0;
        }).AddEntityFrameworkStores<LearningLanternContext>().AddDefaultTokenProviders();
        return services;
    }

    private static IServiceCollection AddMediatRConfiguration(this IServiceCollection services)
    {
        services.AddMediatR(Assembly.GetExecutingAssembly());
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AuthorizedBehavior<,>));
        return services;
    }


    private static IServiceCollection AddValidations(this IServiceCollection services)
    {
        services.AddFluentValidation(
            fv => { fv.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly()); }
        );
        return services;
    }

    private static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddSingleton<ICurrentUserService, CurrentUserService>();

        services.AddSingleton<IEmailSender, EmailSender>(
            op => new EmailSender(ConfigProvider.MyEmail, ConfigProvider.MyPassword,
                ConfigProvider.SmtpServerAddress, ConfigProvider.MailPort)
        );
        services.AddSingleton<IEventProcessor, EventProcessor>();
        services.AddTransient<IClassroomRepository, ClassroomRepository>();
        return services;
    }

    public static IApplicationBuilder AddRabbitMQConfiguration(this IApplicationBuilder app)
    {
        var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
        eventBus.AddEvent<UpdateUserEvent>("Users");
        eventBus.AddEvent<CreateUserEvent>("Users");
        eventBus.AddEvent<DeleteUserEvent>("Users");
        return app;
    }

    private static IServiceCollection AddRabbitMQConfiguration(this IServiceCollection services)
    {
        IConnectionFactory connectionFactory = new ConnectionFactory
        {
            HostName = ConfigProvider.RabbitMQHost,
            Port = ConfigProvider.RabbitMQPortNumber
        };
        services.AddRabbitMQ(connectionFactory);
        return services;
    }

    private static IServiceCollection AddCorsForAngular(this IServiceCollection services)
    {
        services.AddCors(setupAction => setupAction.AddDefaultPolicy(
            policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));
        return services;
    }
}