
namespace bbv_MicroserviceDemo.Messaging.Mqtt
{
    using System;

    public class MessageReceivedArgs : EventArgs
    {
        public string Topic { get; set; }
        public string Message { get; set; }
        public ulong DeliveryTag { get; set; }
    }

    public delegate void MessageReceivedHandler(object sender, MessageReceivedArgs e);
    public interface IMqttClient : IDisposable
    {
        event MessageReceivedHandler MessageReceived;
        void Subscribe(string topic);
        void Publish<TMessage>(string topic, TMessage message);
        void UnSubscribe(string topic);
        void Start();
        void Stop();
        void Ack(ulong deliveryTag);
    }
}
