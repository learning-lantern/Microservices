using RabbitMQ.Client;

namespace LearningLantern.EventBus.RabbitMQConnection;

public interface IRabbitMQConnection : IDisposable
{
    bool IsConnected { get; }

    bool TryConnect();

    IModel CreateModel();
}