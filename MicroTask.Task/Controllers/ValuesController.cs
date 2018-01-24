using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Steeltoe.Discovery.Client;

namespace MicroTask.Task.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private readonly DiscoveryHttpClientHandler _handler;
        private const string WORK_JOURNAL_URLS = "http://WorkJournal/api/values";

        public ValuesController(IDiscoveryClient client)
        {
            this._handler = new DiscoveryHttpClientHandler(client);

        }

        // GET api/values
        [HttpGet("WorkJournal")]
        public async Task<string> Get()
        {
            var client = new HttpClient(_handler,false);

            return await client.GetStringAsync(WORK_JOURNAL_URLS);
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
