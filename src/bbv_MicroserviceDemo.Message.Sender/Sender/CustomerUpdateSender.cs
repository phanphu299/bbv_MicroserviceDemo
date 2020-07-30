
namespace bbv_MicroserviceDemo.Message.Sender.Sender
{
    using bbv_MicroserviceDemo.Messaging.Messages;
    using bbv_MicroserviceDemo.Messaging.Mqtt;

    public class CustomerUpdateSender : ICustomerUpdateSender
    {
        private readonly IMqttClient _mqttClient;

        public CustomerUpdateSender(IMqttClient mqttClient)
        {
            _mqttClient = mqttClient;
        }

        public void SendUpdateCustomer(UpdateCustomerMessage customer)
        {
            _mqttClient.Start();

            _mqttClient.Publish(TopicConstants.UpdateCustomerRequestTopic, customer);

            _mqttClient.Stop();
        }
    }
}
