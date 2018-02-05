using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Business;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Steeltoe.Common.Discovery;
using Models = Business.Models;

namespace MicroTask.WorkTask.Controllers
{
    [Route("workTask/[controller]")]
    [Authorize]
    public class WorkTaskController : Controller
    {
        private readonly DiscoveryHttpClientHandler _handler;
        private readonly IConfigurationRoot _config;
        private readonly IOptionsSnapshot<Demo> _springCloudConfig;
        private readonly IDiscoveryClient _client;


        public WorkTaskController(IDiscoveryClient client,IConfigurationRoot config, IOptionsSnapshot<Demo> configDemo)
        {
            _client = client;
            _config = config;
            _springCloudConfig = configDemo;
        }

        // GET api/task
        [HttpGet()]
        public async Task<string> Get()
        {
            // var client = new HttpClient(_handler,false);
            // return await client.GetStringAsync(WORK_JOURNAL_URLS);

            var configValue = _springCloudConfig.Value;

            return configValue.Name;
        }

        // GET api/task/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/task/{id:int} 
        // header:application/json   
        // body:json

        [HttpPost("{id:int}")]
        public JsonResult Post(int id,[FromBody]Models.WorkTask task)
        {
            if (task == null)
            {
                throw new ArgumentNullException(nameof(task));
            }

            return Json(new { name=task.TaskName }); 
        }

        // PUT api/task/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/task/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
