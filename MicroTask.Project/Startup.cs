using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Pivotal.Discovery.Client;
using Common.Cache.Redis;
using Common.Loggers.Log4Net;
using Infrastructure;
using Business;
using System;
using Domain;

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
            
            // add health check
            // services.AddHealthConfigurationByMetrics(Configuration);

            // add cap
            services.AddCapWithMySQLAndRabbit(Configuration);

            // add spring cloud discovery clientd
            services.AddDiscoveryClient(Configuration);

            // add resource auth service
            services.ConfigureResourceAuthService(Configuration);

            // inject action logger filter instance
            services.AddSingleton<ActionLoggerFilter>();

            // add mvc,include filters and etc..
            services.AddMvcCore(x =>
            {
                x.Filters.Add(typeof(ExcepitonFilter));
            })
            .AddControllersAsServices()
            .AddAuthorization()
            .AddJsonFormatters();

            // add Cors in header,method and credentials
            services.AddCors(options => {
                options.AddPolicy("CORS",
                  builder => builder.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader()
                  .AllowCredentials());
            });

            // add redis cache
            services.AddRedisCache(Configuration);

            // add redis session
            //services.AddRedisSession(Configuration);

            // add memory session
            services.AddMemorySession(Configuration);

            // add chole orm , or you can use another orm tools instead
            services.AddChloeWithMySQL(Configuration);

            // injeciton logic repository and service for business
            services.AddTransient<IProjectRespository, ProjectRespository>();
            services.AddTransient<IProjectService, ProjectService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory,IApplicationLifetime lifeTime)
        {
            // use session
            app.UseSession();

            // use log4net
            loggerFactory.AddLog4Net();

            // use health check by app metrics
            // app.UseHealthConfigurationByMetrics(lifeTime);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
                
            // use authentication
            app.UseAuthentication();

            // use cross domain policy
            app.UseCors("CORS");

            // use mvc
            app.UseMvc();

            // use cap
            app.UseCap();

            // use Pivotal discovery client
            app.UseDiscoveryClient();


            // use forbidden middleware
            app.UseForbiddenMiddleware(Configuration, loggerFactory.CreateLogger<Exception>());
        }

    }

}
