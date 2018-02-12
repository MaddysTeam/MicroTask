using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Steeltoe.Common.Discovery;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace MicroTask.Project.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private readonly DiscoveryHttpClientHandler handler;
        private readonly IConfiguration config;

        public ValuesController(IDiscoveryClient client,IConfiguration configuration)
        {
            handler = new DiscoveryHttpClientHandler(client);
            config = configuration;
        }

        // GET api/values
        [HttpGet]
        [ActionLoggerFilter]
        public async Task<IEnumerable<string>> Get()
        {
            var client = config.GetSection("Identity:Client").Value;
            var secret = config.GetSection("Identity:Secret").Value;
            var authority = config.GetSection("Identity:Authority").Value;
            var cacheServiceApi = "CacheApi";

            var accessTokenResponse=await AuthService.RequestAccesstokenAsync(
                 new AuthTokenRequest(authority, client, secret, cacheServiceApi, handler)
                 );

            var httpClient = new HttpClient();
            httpClient.SetBearerToken(accessTokenResponse.AccessToken);

            var responseMessage = await httpClient.GetAsync("http://localhost:3001/redis/cache/get?key=abcd&value=dddd");

            //return responseMessage;
            //throw new ProjectExcption()
            return new string[] { "value2001", "value2002" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
