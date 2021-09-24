using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MMT.CustomerAccountDetailsApiClient;
using MMT.CustomerApiClient.Interfaces;
using MMT.DbClient;
using MMT.DbClient.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MMT.WebApi
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IConfigurationRoot ConfigurationRoot { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            builder.AddEnvironmentVariables();
            ConfigurationRoot = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            InjectDependancies(services);
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
        }

        private void InjectDependancies(IServiceCollection services)
        {
            services.AddSingleton(ConfigurationRoot);

            services.AddScoped<ICustomerProvider, CustomerProvider>();
    //        services.AddScoped<ICustomerProvider>(sp =>
    //new CustomerProvider(Configuration.GetConnectionString("OrdersDb")));

            services.AddScoped<IHttpClientWrapper, HttpClientWrapper>();
            services.AddScoped<IOrderRepo, OrderRepo>();

            //services.AddScoped<IOrderRepo>(sp =>
            //    new OrderRepo(ConfigurationRoot));
        }
    }
}
