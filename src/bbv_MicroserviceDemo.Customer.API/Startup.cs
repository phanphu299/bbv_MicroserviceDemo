

namespace bbv_MicroserviceDemo.Customer.API
{
    using AutoMapper;
    using bbv_MicroserviceDemo.Customer.API.DataAccess;
    using bbv_MicroserviceDemo.Customer.API.Events.Extensions;
    using bbv_MicroserviceDemo.Customer.API.Misc.Extensions;
    using FluentValidation.AspNetCore;
    using MediatR;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using System;
    using System.IO;
    using System.Reflection;
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.AddControllers();

            services.AddDbContext<CustomerContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            //AutoMapper
            // services.AddAutoMapper(typeof(GetAllCustomer.Profile), typeof(CreateCustomer.Profile));
            services.AddAutoMapper(Assembly.Load("bbv_MicroserviceDemo.Customer.API.Events"));

            //FluentValidation
            services
                .AddMvc()
                .AddFluentValidation(cfg => { cfg.RegisterValidatorsFromAssembly(Assembly.GetAssembly(typeof(ValidatorExtensions))); });

            services.AddSwaggerGen(c =>
            {
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
                c.CustomSchemaIds(i => i.FullName);
            });

            services.RegisterDependencies(Configuration);

            //MediatR
            //services.AddMediatR(Assembly.GetExecutingAssembly(), typeof(GetAllCustomer.QueryHandler).Assembly, typeof(CreateCustomer.CommandHandler).Assembly, typeof(UpdateCustomer.CommandHandler).Assembly);
            services.AddMediatR(Assembly.Load("bbv_MicroserviceDemo.Customer.API.Events"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Customer API V1");
                c.RoutePrefix = string.Empty;
            });
        }
    }
}
