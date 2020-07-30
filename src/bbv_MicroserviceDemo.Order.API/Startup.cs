

namespace bbv_MicroserviceDemo.Order.API
{
    using AutoMapper;
    using bbv_MicroserviceDemo.Order.API.DataAccess;
    using bbv_MicroserviceDemo.Order.API.Events.Extensions;
    using bbv_MicroserviceDemo.Order.API.Misc.Extensions;
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
    using FluentValidation.AspNetCore;

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

            services.AddDbContext<OrderContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            //AutoMapper
            services.AddAutoMapper(Assembly.Load("bbv_MicroserviceDemo.Order.API.Events"));

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
            services.AddMediatR(Assembly.Load("bbv_MicroserviceDemo.Order.API.Events"));
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
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Order API V1");
                c.RoutePrefix = string.Empty;
            });
        }
    }
}
