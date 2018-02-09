using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Steeltoe.Common.Discovery;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MicroTask.Project.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private readonly DiscoveryHttpClientHandler _handler;
        public ValuesController(IDiscoveryClient client)
        {
            _handler = new DiscoveryHttpClientHandler(client);
        }

        // GET api/values
        [HttpGet]
        [ActionLoggerFilter]
        public async Task<IEnumerable<string>> Get()
        {
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
