using System.Text;
using LearningLantern.EventBus.EventProcessor;
using LearningLantern.EventBus.Events;
using LearningLantern.EventBus.RabbitMQConnection;
using Newtonsoft.Json;
using RabbitMQ.Client;
using Serilog;

namespace LearningLantern.EventBus.Publisher;

public class RabbitMQBus : IEventBus
{
    private const string ExchangeName = "LearningLantern";
    private readonly IRabbitMQConnection _connection;
    private readonly IEventProcessor _eventProcessor;
    private IModel _channel;

    public RabbitMQBus(IRabbitMQConnection connection, IEventProcessor eventProcessor)
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
            Log.Logger.Debug("create channel done");
        }
        catch (Exception ex)
        {
            return false;
        }

        return true;
    }


    public void Publish(IntegrationEvent @event)
    {
        if (!_connection.IsConnected) return;
        var message = JsonConvert.SerializeObject(@event);
        var properties = _channel.CreateBasicProperties();
        properties.Persistent = true;
        var body = Encoding.UTF8.GetBytes(message);
        _channel.BasicPublish(ExchangeName, @event.GetType().Name, properties, body);
        Log.Logger.Debug($"event sending, {@event.GetType().Name} = {JsonConvert.SerializeObject(@event)}");
    }

    public void AddEvent<T>(string queueName)
        where T : IntegrationEvent
    {
        if (!_connection.IsConnected) return;

        _channel.QueueDeclare(queueName, true, false, false);
        _channel.QueueBind(queueName, ExchangeName, typeof(T).Name);
    }
}