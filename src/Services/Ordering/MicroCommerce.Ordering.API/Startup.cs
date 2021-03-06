﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MicroCommerce.Shared;
using MicroCommerce.Shared.Grpc;
using MicroCommerce.Shared.Identity;
using MicroCommerce.Shared.Monitoring;
using Microsoft.Extensions.Configuration;
using Prometheus;
using Serilog;

namespace MicroCommerce.Ordering.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddDapr();

            services.AddIdentityAuthentication();

            services.AddSwagger();
            services.AddMonitoring();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwaggerEndpoint();
            }

            app.UseSerilogRequestLogging();

            app.UseRouting();

            app.UseMonitoring();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks();
                endpoints.MapMetrics();
                endpoints.MapControllers();
            });
        }
    }
}
