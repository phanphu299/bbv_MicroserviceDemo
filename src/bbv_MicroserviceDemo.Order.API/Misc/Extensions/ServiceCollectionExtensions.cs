
namespace bbv_MicroserviceDemo.Order.API.Misc.Extensions
{
    using bbv_MicroserviceDemo.Order.API.DataAccess;
    using bbv_MicroserviceDemo.Repositories;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtensions
    {
        public static void RegisterDependencies(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddScoped<DbContext, OrderContext>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        }
    }
}
