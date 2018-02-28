using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pivotal.Discovery.Client;
using Steeltoe.Common.Discovery;
using System.Threading.Tasks;

namespace Infrastructure
{

    public static class AuthService
    {
        /// <summary>
        /// auth configuration from identity server 
        /// </summary>
        /// <param name="services">services</param>
        /// <param name="configuration">configuration</param>
        public static void ConfigureAuthService(this IServiceCollection services, IConfiguration configuration)
        {
            var discoveryClient = services.BuildServiceProvider().GetService<IDiscoveryClient>();
            var handler = new DiscoveryHttpClientHandler(discoveryClient);
            var authority = configuration.GetSection("Identity:Authority").Value;
            var apiName = configuration.GetSection("Identity:Api").Value;
            var secret = configuration.GetSection("Identity:Secret").Value;

           // services.AddDiscoveryClient(configuration);
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

        /// <summary>
        /// get access token from identity server
        /// </summary>
        /// <param name="request">AuthTokenRequest</param>
        /// <returns>AuthTokenResponse</returns>
        public static async Task<AuthTokenResponse> RequestAccesstokenAsync(AuthTokenRequest request, AuthType authType)
        {
            AuthTokenResponse authTokenResponse;
            var clientHandler = request.HttpClientHandler;
            var client = new DiscoveryClient(request.Authority, clientHandler)
            {
                Policy = new DiscoveryPolicy { RequireHttps = false }
            };

            var response = await client.GetAsync();
            if (response.IsError)
            {
               return authTokenResponse = new AuthTokenResponse("", false, response.Error);
            }

            TokenResponse tokenResponse;
            var tokenClient = new TokenClient(response.TokenEndpoint, request.Client, request.Secret, clientHandler);
            if (authType == AuthType.byCredential)
                tokenResponse = await tokenClient.RequestClientCredentialsAsync(request.Api);
            else
                tokenResponse = await tokenClient.RequestResourceOwnerPasswordAsync(request.UserName, request.Password, request.Api);

            if (tokenResponse.IsError)
            {
                authTokenResponse = new AuthTokenResponse("", tokenResponse.IsError, tokenResponse.Error);
            }

            return new AuthTokenResponse(tokenResponse.AccessToken, true, null);
        }
    }
}


// TODO: will delete 
// Console.WriteLine(tokenResponse.Json);

//var httpClient = new HttpClient();
//httpClient.SetBearerToken(tokenResponse.AccessToken);

//responseMessage = await httpClient.GetAsync("http://localhost:5001/api/values");

//return responseMessage;