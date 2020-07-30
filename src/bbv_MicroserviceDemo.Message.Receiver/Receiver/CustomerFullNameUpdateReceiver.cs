
namespace bbv_MicroserviceDemo.Message.Receiver.Receiver
{
    using bbv_MicroserviceDemo.Domains.Entities;
    using bbv_MicroserviceDemo.Messaging.Messages;
    using bbv_MicroserviceDemo.Messaging.Mqtt;
    using bbv_MicroserviceDemo.Repositories;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Newtonsoft.Json;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class CustomerFullNameUpdateReceiver : IHostedService
    {
        private IMqttClient _mqttClient;
        private readonly IServiceScopeFactory _scopeFactory;

        public CustomerFullNameUpdateReceiver(IMqttClient mqttClient, IServiceScopeFactory scopeFactory)
        {
            _mqttClient = mqttClient;
            _mqttClient.MessageReceived += OnMessageReceived;
            _scopeFactory = scopeFactory;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _mqttClient.Start();
            _mqttClient.Subscribe(TopicConstants.UpdateCustomerRequestTopic);

            Console.WriteLine("CustomerFullNameUpdateReceiver started...");
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _mqttClient.Stop();

            Console.WriteLine("Service stopped...");
            return Task.CompletedTask;
        }

        private async void OnMessageReceived(object sender, MessageReceivedArgs e)
        {
            Console.WriteLine($"Message Receive: topic: {e.Topic}, message: {e.Message}");

            if (e.Topic == TopicConstants.UpdateCustomerRequestTopic)
            {
                var message = JsonConvert.DeserializeObject<UpdateCustomerMessage>(e.Message);

                using (var scope = _scopeFactory.CreateScope())
                {
                    try
                    {
                        var _repository = scope.ServiceProvider.GetRequiredService<IRepository<Order>>();

                        var order = await _repository
                                .GetAll()
                                .AsNoTracking()
                                .FirstOrDefaultAsync(x => x.CustomerGuid == message.Id);

                        if (order == null)
                            return;

                        order.CustomerFullName = $"{message.FirstName} {message.LastName}";
                        await _repository.UpdateAsync(order);

                        Console.WriteLine($"Updated.");
                        _mqttClient.Ack(e.DeliveryTag);
                    }
                    catch (Exception ex)
                    {

                        Console.WriteLine(ex.Message);
                    }
                }
            }
        }
    }
}
