
namespace bbv_MicroserviceDemo.Messaging.Mqtt
{
    using Microsoft.Extensions.Options;
    using Newtonsoft.Json;
    using RabbitMQ.Client;
    using RabbitMQ.Client.Events;
    using System.Text;

    public class MqttClient : IMqttClient
    {
        private IConnection _connection;
        private IModel _channel;
        private EventingBasicConsumer _consumer;
        private string _queueName;
        private MessageBrokerSettings _messageBrokerSettings;

        public event MessageReceivedHandler MessageReceived;

        public MqttClient(IOptions<MessageBrokerSettings> messageBrokerSettings)
        {
            _messageBrokerSettings = messageBrokerSettings.Value;
        }

        public void Dispose()
        {
            this.Stop();
        }

        public void Publish<TMessage>(string topic, TMessage message)
        {
            var properties = _channel.CreateBasicProperties();
            properties.Persistent = true;

            string body = JsonConvert.SerializeObject(message);
            _channel.BasicPublish(exchange: MqttConstants.MqttTopic, routingKey: topic, basicProperties: properties, body: Encoding.UTF8.GetBytes(body));
        }

        public void Start()
        {
            var factory = new ConnectionFactory()
            {
                HostName = _messageBrokerSettings.Hostname,
                UserName = _messageBrokerSettings.UserName,
                Password = _messageBrokerSettings.Password,
                AutomaticRecoveryEnabled = true,
                VirtualHost = "/"
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _queueName = _channel.QueueDeclare().QueueName;
            _consumer = new EventingBasicConsumer(_channel);
            _consumer.Received += OnMessageReceived;
        }

        public void Stop()
        {
            if (_channel != null)
            {
                _channel.Close();
                _channel.Dispose();
                _channel = null;
            }

            if (_connection != null)
            {
                _connection.Close();
                _connection.Dispose();
                _connection = null;
            }
        }

        public void Subscribe(string topic)
        {
            _channel.ExchangeDeclare(exchange: MqttConstants.MqttTopic, type: ExchangeType.Topic, durable: true);
            _channel.QueueBind(queue: _queueName, exchange: MqttConstants.MqttTopic, routingKey: topic);
            _channel.BasicConsume(queue: _queueName, true, _consumer);
        }

        public void UnSubscribe(string topic)
        {
            _channel.QueueUnbind(queue: _queueName, exchange: MqttConstants.MqttTopic, routingKey: topic);
        }

        private void OnMessageReceived(object sender, BasicDeliverEventArgs e)
        {
            if (MessageReceived != null)
            {
                var args = new MessageReceivedArgs()
                {
                    Topic = e.RoutingKey,
                    Message = Encoding.UTF8.GetString(e.Body.IsEmpty ? null : e.Body.ToArray())
                };
                MessageReceived(this, args);
            }
        }

        public void Ack(ulong deliveryTag)
        {
            _channel.BasicAck(deliveryTag: deliveryTag, multiple: false);
        }
    }
}
