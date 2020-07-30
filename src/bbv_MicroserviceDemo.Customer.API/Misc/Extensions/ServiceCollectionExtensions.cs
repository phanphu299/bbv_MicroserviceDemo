namespace bbv_MicroserviceDemo.Customer.API.Misc.Extensions
{
    using bbv_MicroserviceDemo.Customer.API.DataAccess;
    using bbv_MicroserviceDemo.Message.Sender.Sender;
    using bbv_MicroserviceDemo.Messaging;
    using bbv_MicroserviceDemo.Messaging.Mqtt;
    using bbv_MicroserviceDemo.Repositories;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtensions
    {
        public static void RegisterDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<DbContext, CustomerContext>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddTransient<ICustomerUpdateSender, CustomerUpdateSender>();
            services.AddScoped<IMqttClient, MqttClient>();

            services.Configure<MessageBrokerSettings>(configuration.GetSection(nameof(MessageBrokerSettings)));
            services.AddScoped<MessageBrokerSettings, MessageBrokerSettings>();
        }
    }
}
