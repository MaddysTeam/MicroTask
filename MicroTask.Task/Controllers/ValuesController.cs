using Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Session;
using Steeltoe.Common.Discovery;
using System.Threading.Tasks;
using System.Net.Http;

namespace Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private readonly DiscoveryHttpClientHandler handler;
        private readonly IConfiguration config;
        private IRedisCache cache;

        public ValuesController(IDiscoveryClient client,IConfiguration configuration,IRedisCache redisCache)
        {
            handler = new DiscoveryHttpClientHandler(client);
            config = configuration;
            cache = redisCache;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login()
        {
            var client = config.GetSection("Identity:Client").Value;
            var secret = config.GetSection("Identity:Secret").Value;
            var authority = config.GetSection("Identity:Authority").Value;
            var projectApi = config.GetSection("Identity:Api").Value;

            var response = await AuthService.RequestAccesstokenAsync(
                 new AuthTokenRequest(authority, client, secret, projectApi, "tom", "aaa", handler),
                 AuthType.byResoucePassword);

            return Ok(new
            {
                AccessToken = response.AccessToken,
            });
        }

        // GET api/values
        [HttpGet]
        [ActionLoggerFilter]
        public async Task<string> Get()
        {
            //var client = config.GetSection("Identity:Client").Value;
            //var secret = config.GetSection("Identity:Secret").Value;
            //var authority = config.GetSection("Identity:Authority").Value;
            //var taskApi = config.GetSection("Identity:Api").Value;

            //var accessTokenResponse = await AuthService.RequestAccesstokenAsync(
            //     new AuthTokenRequest(authority, client, secret, taskApi, "tom","aaa", handler),
            //     AuthType.byResoucePassword);

            //var httpClient = new HttpClient();
            //httpClient.SetBearerToken(accessTokenResponse.AccessToken);

            //var responseMessage =await httpClient.GetAsync("http://localhost:5555/project/admin");

            //return responseMessage;
            //throw new ProjectExcption()
            //var values = "aaaa";
            //HttpContext.Session.SetString("key", "strValue");
            //cache.Set("aaa", values);
            return string.Empty;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            //var value = cache.Get("aaa");
            //HttpContext.Session.GetString("key");
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
