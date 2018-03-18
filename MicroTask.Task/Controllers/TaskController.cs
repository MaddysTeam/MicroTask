using Business;
using Chloe.MySql;
using Common;
using DotNetCore.CAP;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Steeltoe.Common.Discovery;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WorkTask = Business.Task;

namespace Controllers
{

    [Route("task")]
    public class TaskController : Controller
    {

        public TaskController(IDiscoveryClient client,ITaskService taskService, MySqlContext context)
        {
            _handler = new DiscoveryHttpClientHandler(client);
            _taskService = taskService;
        }

        [Authorize(Roles ="admin")]
        [Route("admin")]
        [HttpGet]
        public async Task<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // POST project/edit
        [HttpPost]
        [Route("edit")]
        public void Edit([FromBody]WorkTask task)
        {
            if (task.Id.IsNullOrEmpty())
            {
                _taskService.AddTask(task);
            }
        }

        // POST project/{id}
        [HttpGet]
        [Route("{id}")]
        public WorkTask GetProject(string id)
        {
            var task = _taskService.GetTaskById(id);

            return task;
        }

        [CapSubscribe("xxx.project.check")]
        public void SubscribeMessage2(string name)
        {
            Console.WriteLine(name);
        }


        private readonly DiscoveryHttpClientHandler _handler;
        private readonly ITaskService _taskService;
        //private readonly MySqlContext _sqlContext;

    }

}
