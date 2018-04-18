using Business;
using IdentityServer4.Stores;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pivotal.Discovery.Client;

namespace Identity
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
            services.AddDiscoveryClient(Configuration);

            // add mvc,include filters and etc..
            services.AddMvcCore(x =>
            {
               // x.Filters.Add(typeof(ExcepitonFilter));
            })
            .AddControllersAsServices()
            .AddAuthorization()
            .AddJsonFormatters();

            var config = new Config(Configuration);
            services.AddIdentityServer(x =>
                {
                    x.IssuerUri = "http://identity";
                    x.PublicOrigin = "http://identity";
                })
                .AddDeveloperSigningCredential()
                .AddInMemoryPersistedGrants()
                .AddInMemoryApiResources(config.GetApiResources())
                .AddInMemoryClients(config.GetClients());
            services.AddSingleton<IPersistedGrantStore, RedisPersistedGrantStore>();
            services.AddTransient<IResourceOwnerPasswordValidator, ResourceOwnerPasswordValidator>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
            app.UseDiscoveryClient();
            app.UseIdentityServer();

        }
    }
}
