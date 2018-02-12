using Business;
using Microsoft.AspNetCore.Mvc;
using Common;
using Infrastructure;
using Microsoft.AspNetCore.Authorization;

namespace MicroTask.CacheService.Controllers
{
    [Route("redis/cache")]
    [Authorize]
    public class RedisCacheController : Controller
    {

        public RedisCacheController(IRedisCacheStringKeyValueService cacheService)
        {
            this.cacheService = cacheService;
        }

        // POST redis/cache/set

        [HttpGet("set")]
        [ActionLoggerFilter]
        public JsonResult SetCache(string key, string value)
        {
            key.EnsureNotNull(() => new CacheServiceException("key not null"));
            value.EnsureNotNull(() => new CacheServiceException("value not null"));

            var isSuccess = cacheService.SetCache(key, value);

            return Json(new
            {
                isSuccess
            });
        }

        // POST redis/cache/get

        [HttpGet("get")]
        [ActionLoggerFilter]
        public string GetCache(string key)
        {
            var val= cacheService.Get(key);

            return val;
        }

        private IRedisCacheStringKeyValueService cacheService;

    }
}
