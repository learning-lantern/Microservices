using System.Net.Sockets;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using Serilog;

namespace LearningLantern.Common.EventBus.RabbitMQConnection;

public class RabbitMQConnection : IRabbitMQConnection
{
    private const int RetryCount = 3;
    private readonly IConnectionFactory _connectionFactory;
    private readonly object sync_object = new();
    private IConnection _connection;
    private bool _disposed;

    public RabbitMQConnection(IConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public void Dispose()
    {
        if (_disposed) return;

        _disposed = true;

        try
        {
            _connection.Dispose();
        }
        catch (IOException ex)
        {
        }
    }

    public bool IsConnected => _connection is {IsOpen: true} && !_disposed;

    public bool TryConnect()
    {
        if (IsConnected)
            return true;


        lock (sync_object)
        {
            var policy = Policy.Handle<SocketException>()
                .Or<BrokerUnreachableException>()
                .Or<InvalidOperationException>()
                .WaitAndRetry(RetryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                );
            policy.Execute(() =>
            {
                if (IsConnected) return;

                Log.Logger.Information("Try to connect to RabbitMQ");
                _connection = _connectionFactory.CreateConnection();
                if (_connection is {IsOpen: true})
                    Log.Logger.Information("Connection done");
            });
        }

        return IsConnected;
    }

    public IModel CreateModel()
    {
        if (!IsConnected)
            throw new InvalidOperationException("No RabbitMQ connections are available to perform this action");

        return _connection.CreateModel();
    }
}