using System.Text;
using LearningLantern.Common.EventBus.EventProcessor;
using LearningLantern.Common.EventBus.Events;
using LearningLantern.Common.EventBus.RabbitMQConnection;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Serilog;

namespace LearningLantern.Common.EventBus;

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
            if (ExchangeName.Length > 0) _channel.ExchangeDeclare(ExchangeName, ExchangeType.Direct);
            _channel.BasicQos(0, 1, false);
        }
        catch (Exception ex)
        {
            Log.Logger.Debug(ex.Message);
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
    }

    public void AddEvent<T>(string queueName)
        where T : IntegrationEvent
    {
        if (!_connection.IsConnected) return;

        _channel.QueueDeclare(queueName, true, false, false);
        _channel.QueueBind(queueName, ExchangeName, typeof(T).Name);
    }

    public void Subscribe(string queueName)
    {
        if (!_connection.IsConnected) return;

        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (s, eventArgs) =>
        {
            var eventName = eventArgs.RoutingKey;
            var jsonSpecified = Encoding.UTF8.GetString(eventArgs.Body.ToArray());
            await _eventProcessor.ProcessEvent(eventName, jsonSpecified);
            _channel.BasicAck(eventArgs.DeliveryTag, false);
        };
        _channel.BasicConsume(queueName, false, consumer);
    }
}