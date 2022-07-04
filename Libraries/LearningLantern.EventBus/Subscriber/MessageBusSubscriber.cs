using System.Text;
using LearningLantern.EventBus.EventProcessor;
using LearningLantern.EventBus.Exceptions;
using LearningLantern.EventBus.RabbitMQConnection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Serilog;

namespace LearningLantern.EventBus.Subscriber;

public class RabbitMQBusSubscriber : BackgroundService
{
    private const string ExchangeName = "LearningLantern";
    private const string QueueName = "chat";

    private readonly IRabbitMQConnection _connection;
    private readonly IEventProcessor _eventProcessor;
    private IModel _channel;

    public RabbitMQBusSubscriber(IRabbitMQConnection connection, IEventProcessor eventProcessor)
    {
        _connection = connection;
        _eventProcessor = eventProcessor;
    }

    public bool SetupConfiguration()
    {
        _connection.TryConnect();

        if (_connection.IsConnected == false)
            return false;

        try
        {
            _channel = _connection.CreateModel();
            if (ExchangeName.Length > 0) _channel.ExchangeDeclare(ExchangeName, ExchangeType.Direct, true);
            _channel.BasicQos(0, 1, false);
            Log.Logger.Debug("create channel in Subscriber done");
        }
        catch (Exception ex)
        {
            return false;
        }

        return true;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Task.Delay(10000, stoppingToken);
        stoppingToken.ThrowIfCancellationRequested();

        if (SetupConfiguration() == false)
            return Task.CompletedTask;

        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += (s, eventArgs) =>
        {
            try
            {
                var eventName = eventArgs.RoutingKey;
                Log.Logger.Debug($"{eventName} Received");
                var jsonSpecified = Encoding.UTF8.GetString(eventArgs.Body.ToArray());
                Log.Logger.Debug(jsonSpecified);
                _eventProcessor.ProcessEvent(eventName, jsonSpecified).Wait(stoppingToken);
                _channel.BasicAck(eventArgs.DeliveryTag, false);
            }
            catch (UnhandledEventException ex)
            {
                Log.Logger.Debug(ex.Message);
            }
            catch (Exception ex)
            {
                Log.Logger.Debug(ex.Message);
            }
        };
        _channel.BasicConsume(QueueName, false, consumer);
        return Task.CompletedTask;
    }
}