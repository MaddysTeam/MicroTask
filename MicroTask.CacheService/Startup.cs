using Business;
using Camoran.Redis.Cache;
using Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Pivotal.Discovery.Client;

namespace MicroTask.CacheService
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
            services.AddMvc();
            services.AddDiscoveryClient(Configuration);

            //auth service
            services.ConfigureAuthService(Configuration);

            //bussiness service
            services.AddTransient<ICahceStrategy<string, string>, RedisEncryptKeyStrategy<string>>();
            services.AddTransient(typeof(RedisStringCache));
            services.AddTransient<IRedisCacheStringKeyValueService, RedisStringCacheService>();
        }   

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // use identity authentication
            app.UseAuthentication();

            // use mvc
            app.UseMvc();

            // use Pivotal discovery client
            app.UseDiscoveryClient();

            // use log
            app.UseNetCoreLogger(loggerFactory, LogType.Console, Configuration);
        }

    }

}
