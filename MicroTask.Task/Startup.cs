﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Pivotal.Discovery.Client;
using Infrastructure;
using Business;
using System;

namespace MicroTask.Task
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
            services.AddHealthConfigurationByMetrics(Configuration);
            // add auth service
            services.AddDiscoveryClient(Configuration);
            services.ConfigureAuthService(Configuration);

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
                options.AddPolicy("CorsPolicy",
                  builder => builder.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader()
                  .AllowCredentials());
            });

            // add redis cache
            services.AddRedisCache(Configuration);

            // add redis session
            //services.AddDistributedRedisCache(option => {
            //    option.Configuration = Configuration.GetSection("RedisSessionSettings:conn").Value;
            //    option.InstanceName = "master";
            //});
            //services.AddSession();

            // add chole orm 
            services.AddChloeWithMySQL(Configuration);

            // injeciton logic repository and service for business
            services.AddTransient<ITaskRespository, TaskRespository>();
            services.AddTransient<ITaskService, TaskService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory,IApplicationLifetime lifeTime)
        {
            // use session
            // app.UseSession();

            // use log
            app.UseNetCoreLogger(loggerFactory, LogType.Console, Configuration);

            // use health check by app metrics
            app.UseHealthConfigurationByMetrics(lifeTime);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // use authentication
            app.UseAuthentication();

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
