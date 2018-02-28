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

<<<<<<< HEAD
            // add redis cache
            services.AddRedisCache(Configuration);

            // add redis session
            //services.AddDistributedRedisCache(option => {
            //    option.Configuration = Configuration.GetSection("RedisSessionSettings:conn").Value;
            //    option.InstanceName = "master";
            //});
            //services.AddSession();
=======
            // add auth service
            services.AddDiscoveryClient(Configuration);
            services.ConfigureAuthService(Configuration);

            // add chole 
            services.AddScoped(provider => {
                string connString = Configuration.GetSection("ConnectStrings:MySql").Value;

                return new Chloe.MySql.MySqlContext(new MysqlConnectionFactory(connString));
            });
>>>>>>> 278b165bb380ca9f49c35c7c140860fa2b897ab5

            // injeciton logic repository and service for business
            services.AddTransient<IProjectRespository, ProjectRespository>();
            services.AddTransient<IProjectService, ProjectService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            // use session
            // app.UseSession();

            // use log
            app.UseNetCoreLogger(loggerFactory, LogType.Console, Configuration);

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
