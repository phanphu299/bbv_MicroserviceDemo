
namespace bbv_MicroserviceDemo.Message.Sender.Sender
{
    using bbv_MicroserviceDemo.Messaging.Messages;

    public interface ICustomerUpdateSender
    {
        void SendUpdateCustomer(UpdateCustomerMessage customer);
    }
}
