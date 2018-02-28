using Business;
using Microsoft.AspNetCore.Mvc;
using Steeltoe.Common.Discovery;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common;
using Chloe.MySql;

namespace Controllers
{

    [Route("project")]
    public class ProjectController : Controller
    {

        public ProjectController(IDiscoveryClient client,IProjectService projectService, MySqlContext context)
        {
            this.handler = new DiscoveryHttpClientHandler(client);
            this.projectService = projectService;
        }

<<<<<<< HEAD
        // GET api/values
        [Authorize(Roles ="admin")]
        [Route("admin")]
        [HttpGet]
        public async Task<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // POST parent/regist
=======
        // GET project/values/all
>>>>>>> 278b165bb380ca9f49c35c7c140860fa2b897ab5
        [HttpPost]
        [Route("all")]
        public async Task<IEnumerable<Project>> Get()
        {
            return null; //new string[] { "value1", "value2" };
        }

        // POST project/edit
        [HttpPost]
        [Route("edit")]
        public void Edit([FromBody]Project project)
        {
            if (project.Id.IsNullOrEmpty())
            {
                projectService.AddProject(project);
            }
        }


        // POST project/{id}
        [HttpGet]
        [Route("{id}")]
        public Project GetProject(string id)
        {
            var project = projectService.GetProjectById(id);

            return project;
        }


        private readonly DiscoveryHttpClientHandler handler;
        private readonly IProjectService projectService;
        private readonly MySqlContext sqlContext;

    }

}
