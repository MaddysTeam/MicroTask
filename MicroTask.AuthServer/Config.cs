using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4;
using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;

namespace Identity
{
    public class Config
    {
        private readonly IConfiguration _configuration;

        public Config(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };
        }

        public IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("ProjectApi", "PROJECT API")
                {
                    //ApiSecrets = {new Secret("secret".Sha256())},
                    //UserClaims = new List<string>{"role"}
                },
                new ApiResource("UserApi","USER API")
                {
                    //ApiSecrets = {new Secret("secret".Sha256())},
                    //UserClaims = new List<string>{"role"}
                }
            };
        }

        public IEnumerable<Client> GetClients()
        {
            var accessTokenLifetime = int.Parse(_configuration.GetConnectionString("AccessTokenLifetime"));

            // clients
            return new List<Client>
            {
                // for client credentials by using jwt token type
                new Client
                {
                    ClientId = "client",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = { "UserApi","ProjectApi" },
                    AccessTokenType = AccessTokenType.Jwt
                },
                // for owner password by using reference token type
                new Client
                {
                    ClientId = "client.reference",
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    AllowOfflineAccess = true,
                    AccessTokenLifetime = accessTokenLifetime,
                    AllowedScopes = { "UserApi","ProjectApi" },
                    AccessTokenType = AccessTokenType.Reference
                },
            };
        }
    }
}