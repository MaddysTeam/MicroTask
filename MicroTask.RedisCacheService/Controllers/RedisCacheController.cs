using Business;
using Microsoft.AspNetCore.Mvc;

namespace MicroTask.CacheService.Controllers
{
    [Route("redis/cache")]
    public class RedisCacheController : Controller
    {

        public RedisCacheController(IRedisCacheStringKeyValueService cacheService)
        {
            this.cacheService = cacheService;
        }

        // POST redis/cache/set

        [HttpPost("set")]
        public OkResult SetCache(string key,string value)
        {
            cacheService.SetCache(key, value);

            return Ok();
        }

        // POST redis/cache/get

        [HttpPost("get")]
        public string GetCache(string key)
        {
            return cacheService.Get(key);
        }

        private IRedisCacheStringKeyValueService cacheService;

    }
}
