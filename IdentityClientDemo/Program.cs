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
            CallApi();
            Console.Read();
        }


        static async Task GetToken()
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

        static async void CallApi()
        {
            var token = "eyJhbGciOiJSUzI1NiIsImtpZCI6ImFhY2IyM2NkOTIxYjYyOGYzY2FhMjhhODBhYWI4NjRlIiwidHlwIjoiSldUIn0.eyJuYmYiOjE1MTY4OTA4MzMsImV4cCI6MTUxNjg5NDQzMywiaXNzIjoiaHR0cDovL2xvY2FsaG9zdDo5MDAwIiwiYXVkIjpbImh0dHA6Ly9sb2NhbGhvc3Q6OTAwMC9yZXNvdXJjZXMiLCJhcGkxIl0sImNsaWVudF9pZCI6ImNsaWVudCIsInNjb3BlIjpbImFwaTEiXX0.DG_lAvqlQw_s9GbhaKt7FSQsGrouGEECWcu-ax_pkXyXY52mwNdAqsBlH3xS3bXoJa6MgVcFQ9XuQ1KCFdhLaRZWaiHG5UQ56HubxbQVVxEmuFbizkVAa1l7KwfC4C5ShwA1m34SwCSZa7tLtLggYfFZb5VyOKsJxQrsx7O8peqKIrSS97YMDHeuxp94R0J7vYKnXbp7m9HVPfWJjTGOWcjbUCQJ9sGIgMBFrXBg4J_r2hTyJGOStOFirQAUXOciZrZuoQDic9CX4CZ997XfDjxOdK8AF4dpzOexkpF4Mtxr1t0A1GUIlNLBVYo-6QlUqxJkvmtQEjBhkJLS3WwvQQ";
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
