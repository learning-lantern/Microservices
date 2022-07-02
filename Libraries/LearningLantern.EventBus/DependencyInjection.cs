using LearningLantern.EventBus.RabbitMQConnection;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace LearningLantern.EventBus;

public static class DependencyInjection
{
    public static IServiceCollection AddRabbitMQ(this IServiceCollection services, IConnectionFactory connectionFactory)
    {
        return services.AddSingleton<IRabbitMQConnection, RabbitMQConnection.RabbitMQConnection>(
            op => new RabbitMQConnection.RabbitMQConnection(connectionFactory)
        ).AddSingleton<IEventBus, RabbitMQBus>();
    }
}