namespace bbv_MicroserviceDemo.Customer.API.Misc.Extensions
{
    using bbv_MicroserviceDemo.Customer.API.DataAccess;
    using bbv_MicroserviceDemo.Repositories;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtensions
    {
        public static void RegisterDependencies(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddScoped<DbContext, CustomerContext>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            //services.AddTransient<ICustomerUpdateSender, CustomerUpdateSender>();
        }
    }
}
