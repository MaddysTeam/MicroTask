using IdentityModel.Client;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace IdentityClientDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            //GetTokenByPassword()
            //    .GetAwaiter()
            //    .OnCompleted(() =>
            //    {
            //        Console.WriteLine("complete");

            GetTokenByPassword();
            Console.Read();
        }


        static async void GetTokenByCredential()
        {
            var response = await DiscoveryClient.GetAsync("http://localhost:9000");
            if (response.IsError)
            {
                Console.WriteLine(response.Error);
            }

            var tokenClinet = new TokenClient(response.TokenEndpoint, "client", "secret");
            var tokenResponse = await tokenClinet.RequestClientCredentialsAsync("api1");

            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
            }

            Console.WriteLine(tokenResponse.Json);
        }

        static async Task GetTokenByPassword()
        {
            var response = await DiscoveryClient.GetAsync("http://localhost:9000");
            if (response.IsError)
            {
                Console.WriteLine(response.Error);
            }

            var tokenClinet = new TokenClient(response.TokenEndpoint, "ro.client", "secret");
            var tokenResponse = await tokenClinet.RequestResourceOwnerPasswordAsync("alice", "password","api1");

            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
            }

            Console.WriteLine(tokenResponse.Json);
        }


        static async void CallApi()
        {
            var token = "eyJhbGciOiJSUzI1NiIsImtpZCI6ImFhY2IyM2NkOTIxYjYyOGYzY2FhMjhhODBhYWI4NjRlIiwidHlwIjoiSldUIn0.eyJuYmYiOjE1MTY5MzIyNDYsImV4cCI6MTUxNjkzNTg0NiwiaXNzIjoiaHR0cDovL2xvY2FsaG9zdDo5MDAwIiwiYXVkIjpbImh0dHA6Ly9sb2NhbGhvc3Q6OTAwMC9yZXNvdXJjZXMiLCJhcGkxIl0sImNsaWVudF9pZCI6InJvLmNsaWVudCIsInN1YiI6IjEiLCJhdXRoX3RpbWUiOjE1MTY5MzIyNDYsImlkcCI6ImxvY2FsIiwic2NvcGUiOlsiYXBpMSJdLCJhbXIiOlsicHdkIl19.VM_NjbXOTxJnfhNa7z8bjHYGkKMS8ne1Hu8j0BJQ5tUIZoEQXYWHkXK2uUBQ_-cq05AttnjJFGyUJsb4tWToaS_iw9Lza9VMV7vXLFBnzkhb_-RCEF1ZxXqsc2D568YV6E9Czcvo5pUm3NQTkKG5idBoQ0o9ossm-xotV1MdWbScYybzOH1Tz8NXGbeb3ane8o1HAtB_bBkKoGUyARUtuqVnPOKzj8kLYDPjXqNZvfOv9_ri_yf2kL4WSjTnecn7FRcNZvKk1R93z_ZKeO539ZSk_628_nyUxZ12GCHLt_jLYjWzj7zs7SFxnrsfiuZATbI9yowfoGGUPrDYUme-uw";

            var client = new HttpClient();
            client.SetBearerToken(token);

            var response = await client.GetAsync("http://localhost:5001/api/values");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(JArray.Parse(content));
            }
        }


    }
}
