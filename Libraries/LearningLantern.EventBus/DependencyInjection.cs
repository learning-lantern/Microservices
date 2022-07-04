using LearningLantern.EventBus.Publisher;
using LearningLantern.EventBus.RabbitMQConnection;
using LearningLantern.EventBus.Subscriber;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace LearningLantern.EventBus;

public static class DependencyInjection
{
    public static IServiceCollection AddRabbitMQConnection(
        this IServiceCollection services, IConnectionFactory connectionFactory)
    {
        return services.AddSingleton<IRabbitMQConnection, RabbitMQConnection.RabbitMQConnection>(
            op => new RabbitMQConnection.RabbitMQConnection(connectionFactory)
        );
    }

    public static IServiceCollection AddPublisherRabbitMQ(this IServiceCollection services) =>
        services.AddSingleton<IEventBus, RabbitMQBus>();

    public static IServiceCollection AddSubscriberRabbitMQ(this IServiceCollection services) =>
        services.AddHostedService<RabbitMQBusSubscriber>();
}