﻿using Business;
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

            services.AddIdentity<Account, AccountRole>();
            services.AddTransient<IUserStore<Account>, AccountStore>();
            services.AddTransient<IRoleStore<AccountRole>, AccountRoleStore>();
            services.Configure<IdentityOptions>(options => {
                // identity options 
                options.Password.RequiredLength = 6;
                options.Password.RequireDigit = true;
            });

            services.AddMvc();

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
            services.AddTransient<IAccountServices, AccountIdentityService>();
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
