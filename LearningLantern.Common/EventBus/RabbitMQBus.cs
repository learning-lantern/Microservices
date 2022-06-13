using System.Text;
using LearningLantern.Common.EventBus.EventProcessor;
using LearningLantern.Common.EventBus.Events;
using LearningLantern.Common.EventBus.RabbitMQConnection;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace LearningLantern.Common.EventBus;

public class RabbitMQBus : IEventBus
{
    private const string ExchangeName = "LearningLantern.EventBus";
    private readonly IModel _channel;
    private readonly IRabbitMQConnection _connection;
    private readonly IEventProcessor _eventProcessor;

    public RabbitMQBus(IRabbitMQConnection connection, IEventProcessor eventProcessor)
    {
        _connection = connection;
        _eventProcessor = eventProcessor;
        _channel = connection.CreateModel();
        _channel.ExchangeDeclare(ExchangeName, ExchangeType.Direct);
        _channel.BasicQos(0, 1, false);
    }

    public void Publish(IntegrationEvent @event)
    {
        var message = JsonConvert.SerializeObject(@event);
        var properties = _channel.CreateBasicProperties();
        properties.Persistent = true;
        var body = Encoding.UTF8.GetBytes(message);
        _channel.BasicPublish(ExchangeName, @event.GetType().Name, properties, body);
    }

    public void AddEvent<T>(string queueName)
        where T : IntegrationEvent
    {
        var eventName = typeof(T).Name;
        _channel.QueueDeclare(queueName, true, false, false);
        _channel.QueueBind(queueName, ExchangeName, eventName);
    }

    public void Subscribe(string queueName)
    {
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