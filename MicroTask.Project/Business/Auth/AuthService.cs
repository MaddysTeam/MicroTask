using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Steeltoe.Common.Discovery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Bussiess.Auth
{

    public static class AuthService
    {
        /// <summary>
        /// auth configuration from identity server 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void ConfigureAuthService(this IServiceCollection services, IConfiguration configuration)
        {
            var discoveryClient = services.BuildServiceProvider().GetService<IDiscoveryClient>();
            var handler = new DiscoveryHttpClientHandler(discoveryClient);
            var url = configuration.GetSection("Identity:Url").Value;
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
                x.Authority = url;
                x.RequireHttpsMetadata = false;
                x.JwtBackChannelHandler = handler;
                x.IntrospectionDiscoveryHandler = handler;
                x.IntrospectionBackChannelHandler = handler;
            });
        }


        //public static async Task<string> GetAccesstokenAsync(AuthModel model, DiscoveryHttpClientHandler handler)
        //{
        //    var client = new DiscoveryClient(model.Authority, handler)
        //    {
        //        Policy = new DiscoveryPolicy { RequireHttps = false }
        //    };
        //    var response = await client.GetAsync();
        //    if (response.IsError)
        //    {
        //        model.Error = "";
        //    }

        //    // var tokenClinet = new TokenClient(response.TokenEndpoint, "client", "secret", handler);
        //    var tokenClinet = new TokenClient(response.TokenEndpoint, model.Client, model.Secret, handler);
        //    var tokenResponse = await tokenClinet.RequestClientCredentialsAsync("UserApi");

        //    if (tokenResponse.IsError)
        //    {
        //        Console.WriteLine(tokenResponse.Error);
        //    }

        //    model.AccessToken = tokenResponse.AccessToken;

        //    return tokenResponse.AccessToken;
        //    // Console.WriteLine(tokenResponse.Json);

        //    //var httpClient = new HttpClient();
        //    //httpClient.SetBearerToken(tokenResponse.AccessToken);

        //    //responseMessage = await httpClient.GetAsync("http://localhost:5001/api/values");

        //    //return responseMessage;
        //}
    }
}
