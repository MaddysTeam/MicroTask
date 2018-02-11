using Business;
using Camoran.Redis.Cache;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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

            //bussiness service
            services.AddTransient<ICahceStrategy<string, string>, RedisEncryptKeyStrategy<string>>();
            services.AddTransient(typeof(RedisStringCache));
            services.AddTransient<IRedisCacheStringKeyValueService, RedisStringCacheService>();
        }   

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();

            // use Pivotal discovery client
            app.UseDiscoveryClient();
        }
    }
}
