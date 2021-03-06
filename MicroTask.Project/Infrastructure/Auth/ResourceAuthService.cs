﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Steeltoe.Common.Discovery;

namespace Infrastructure
{

    public static class ResourceAuthService
    {

        /// <summary>
        /// auth configuration from identity server 
        /// </summary>
        /// <param name="services">services</param>
        /// <param name="configuration">configuration</param>
        public static void ConfigureResourceAuthService(this IServiceCollection services, IConfiguration configuration)
        {
            var discoveryClient = services.BuildServiceProvider().GetService<IDiscoveryClient>();
            var handler = new DiscoveryHttpClientHandler(discoveryClient);
            var authority = configuration.GetSection("Identity:Authority").Value;
            var apiName = configuration.GetSection("Identity:Api").Value;
            var secret = configuration.GetSection("Identity:Secret").Value;

            services.AddAuthorization();
            services.AddAuthentication(x =>
            {
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddIdentityServerAuthentication(x =>
            {
                x.ApiName = apiName;
                x.ApiSecret = secret;
                x.Authority = authority;
                x.RequireHttpsMetadata = false;
                x.JwtBackChannelHandler = handler;
                x.IntrospectionDiscoveryHandler = handler;
                x.IntrospectionBackChannelHandler = handler;
            });
        }
 
    }

}