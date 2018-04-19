using Common;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using Microsoft.Extensions.Configuration;
using Steeltoe.Common.Discovery;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Business
{

    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {

        public ResourceOwnerPasswordValidator(IDiscoveryClient client, IConfiguration configuration)
        {
            _handler = new DiscoveryHttpClientHandler(client);
            _configuration = configuration;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            // you can change logic here 
            // for example : this can get user info by name and password , role info as well 

            var claims = new List<Claim>()
                {
                  new Claim("role","admin")
                };

            context.Result =
                new GrantValidationResult(
                    context.UserName,
                    OidcConstants.AuthenticationMethods.Password,
                    claims);

           // context.Result = new GrantValidationResult(TokenRequestErrors.InvalidClient);
        }

        private readonly DiscoveryHttpClientHandler _handler;
        private readonly IConfiguration _configuration;

    }

}
