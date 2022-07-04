using System.Reflection;
using FluentValidation.AspNetCore;
using LearningLantern.ApiGateway.CalendarAggregator;
using LearningLantern.ApiGateway.Classroom.Events;
using LearningLantern.ApiGateway.Data;
using LearningLantern.ApiGateway.Data.Models;
using LearningLantern.ApiGateway.PipelineBehaviors;
using LearningLantern.ApiGateway.Utility;
using LearningLantern.Common.Extensions;
using LearningLantern.Common.Services;
using LearningLantern.EventBus;
using LearningLantern.EventBus.EventProcessor;
using LearningLantern.EventBus.Events;
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
        services.AddInfrastructure().AddEventsHandler();

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
        services.AddTransient<JWTGenerator>();

        services.AddSingleton<IEmailSender, EmailSender>(
            op => new EmailSender(ConfigProvider.MyEmail, ConfigProvider.MyPassword,
                ConfigProvider.SmtpServerAddress, ConfigProvider.MailPort)
        );
        services.AddHttpClient<CalendarService>();
        services.AddHttpClient<TodoService>();
        
        return services;
    }

    private static IServiceCollection AddEventsHandler(this IServiceCollection services)
    {
        services.AddSingleton<IEventProcessor, EventProcessor>();
        services.AddTransient<IIntegrationEventHandler<NewRoomEvent>, NewRoomEventHandler>();
        services.AddTransient<IIntegrationEventHandler<JoinRoomEvent>, JoinRoomEventHandler>();

        return services;
    }

    private static IServiceCollection AddRabbitMQConfiguration(this IServiceCollection services)
    {
        IConnectionFactory connectionFactory = new ConnectionFactory
        {
            Uri = ConfigProvider.RabbitMQUri
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