using Business;
using Microsoft.AspNetCore.Mvc;
using Steeltoe.Common.Discovery;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common;
using Chloe.MySql;
using Microsoft.AspNetCore.Authorization;
using WorkTask = Business.Task;

namespace Controllers
{

    [Route("project")]
    public class TaskController : Controller
    {

        public TaskController(IDiscoveryClient client,ITaskService projectService, MySqlContext context)
        {
            this.handler = new DiscoveryHttpClientHandler(client);
            this.taskService = projectService;
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
                taskService.AddTask(task);
            }
        }


        // POST project/{id}
        [HttpGet]
        [Route("{id}")]
        public WorkTask GetProject(string id)
        {
            var task = taskService.GetTaskById(id);

            return task;
        }


        private readonly DiscoveryHttpClientHandler handler;
        private readonly ITaskService taskService;
        private readonly MySqlContext sqlContext;

    }

}
