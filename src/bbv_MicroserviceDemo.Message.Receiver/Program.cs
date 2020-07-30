
namespace bbv_MicroserviceDemo.Message.Receiver
{
    using bbv_MicroserviceDemo.Message.Receiver.Receiver;
    using bbv_MicroserviceDemo.Messaging;
    using bbv_MicroserviceDemo.Messaging.Mqtt;
    using bbv_MicroserviceDemo.Repositories;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using System;
    using System.Threading.Tasks;

    class Program
    {
        public static async Task<int> Main(string[] args)
        {
            try
            {
                var host = CreateHostBuilder(args).Build();
                Console.WriteLine("Starting host...");
                await host.RunAsync();
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Host terminated unexpectedly : " + ex.Message);
                return 1;
            }
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {

            return Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    services.AddTransient<IMqttClient, MqttClient>();
                    services.AddDbContext<OrderContext>(options =>
                    {
                        options.UseSqlServer(context.Configuration["ConnectionStrings:DefaultConnection"]);
                    });
                    services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
                    services.AddScoped<DbContext, OrderContext>();
                    services.AddHostedService<CustomerFullNameUpdateReceiver>();

                    services.Configure<MessageBrokerSettings>(context.Configuration.GetSection(nameof(MessageBrokerSettings)));
                    services.AddScoped<MessageBrokerSettings, MessageBrokerSettings>();
                })
                .UseDefaultServiceProvider((context, options) => options.ValidateScopes = false);
        }
    }
}
