using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Pivotal.Discovery.Client;
using Infrastructure;
using Business;
using System;

namespace MicroTask.Project
{

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
            // add mvc,include filters and etc..
            services.AddMvcCore(x =>
            {
                x.Filters.Add(typeof(ExcepitonFilter));
            })
            .AddControllersAsServices()
            .AddAuthorization()
            .AddJsonFormatters();

            // add auth service
            services.AddDiscoveryClient(Configuration);
            services.ConfigureAuthService(Configuration);

            // add Cors in header,method and credentials
            services.AddCors(options=> {
                options.AddPolicy("CorsPolicy",
                  builder => builder.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader()
                  .AllowCredentials());
            });

            // injeciton logic repository and service for business
            services.AddTransient<IProjectRespository, ProjectRespository>();
            services.AddTransient<IProjectService, ProjectService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            // use log
            app.UseNetCoreLogger(loggerFactory, LogType.Console, Configuration);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // use mvc
            app.UseMvc();

            // use Pivotal discovery client
            app.UseDiscoveryClient();

            // use cross domain policy
            app.UseCors("CorsPolicy");

            // use forbidden middleware
            app.UseForbiddenMiddleware(Configuration, loggerFactory.CreateLogger<Exception>());
        }

    }

}
