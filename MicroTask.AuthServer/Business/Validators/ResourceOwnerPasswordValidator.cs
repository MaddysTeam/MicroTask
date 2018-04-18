using Common;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using Steeltoe.Common.Discovery;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Business
{

    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly DiscoveryHttpClientHandler _handler;
        public ResourceOwnerPasswordValidator(IDiscoveryClient client)
        {
            _handler = new DiscoveryHttpClientHandler(client);
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var client = new HttpClient(_handler);
            var url = $"http://localhost:5555/account/Login?name=kissnana&password=123456";
            var result = await client.GetAsync(url);
            if (result.IsSuccessStatusCode)
            {
                var user = await result.Content.ReadAsObjectAsync<dynamic>();
                var claims = new List<Claim>()
                {
                  new Claim("role","admin")
                };
                context.Result =
                  new GrantValidationResult(
                      context.UserName,
                      OidcConstants.AuthenticationMethods.Password,
                      claims);
            }
            else
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidClient);
            }
        }

    }

}
