using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Collections.Generic;

namespace MicroTask.IdentityAuthServer
{

    public class Config 
    {

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("api1", "My API")
            };
        }

        #region [Get clients credentials]
        //public static IEnumerable<Client> GetClients()
        //{
        //    return new List<Client>
        //    {
        //        new Client
        //        {
        //            ClientId="client",
        //            AllowedGrantTypes=GrantTypes.ClientCredentials,
        //            ClientSecrets=
        //            {
        //                new Secret("secret".Sha256())
        //            },
        //            AllowedScopes ={"api1" }
        //         }
        //   };
        //}
        #endregion

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
             {
                 new Client
                 {
                     ClientId="ro.client",
                     AllowedGrantTypes=GrantTypes.ResourceOwnerPassword,
                    // AllowOfflineAccess=true,
                     ClientSecrets =
                     {
                        // new Secret("A30E6E57-086C-43BE-AF79-67ADECDA0A5B".Sha256());
                         new Secret("secret".Sha256())
                     },
                     AllowedScopes = { "api1" },
                    // AccessTokenType=AccessTokenType.Jwt
                 }
             };
        }

        public static List<TestUser> GetUsers()
        {
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "1",
                    Username = "alice",
                    Password = "password"
                },
                new TestUser
                {
                    SubjectId = "2",
                    Username = "bob",
                    Password = "password"
                }
            };
        }

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };
        }

    }

}
