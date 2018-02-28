using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Business;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Steeltoe.Common.Discovery;

namespace MicroTask.Project.Controllers
{

    public class ProjectController : Controller
    {
        static List<Child> children;
        static List<Parent> parents;

        public ProjectController(IDiscoveryClient client,IProjectService projectService)
        {
            _handler = new DiscoveryHttpClientHandler(client);
            _projectService = projectService;

            if (children == null)
                children = new List<Child>();
            if (parents == null)
                parents = new List<Parent>();
        }

        // GET api/values
        [Authorize(Roles ="admin")]
        [Route("admin")]
        [HttpGet]
        public async Task<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // POST parent/regist
        [HttpPost]
        [Route("parent/regist")]
        public void RegistParent([FromBody]Parent parent)
        {
            if (parent != null)
                parents.Add(parent);
        }

        // POST child/regist
        [HttpPost]
        [Route("child/regist")]
        public void RegistChild([FromBody]Child child)
        {
            if (child != null)
                children.Add(child);
        }

        // POST child/id
        [HttpPost]
        [Route("child/{id}")]
        public Child GetChild(int id)

        {
            return children.Find(x => x.Id == id.ToString());
        }

        // POST children/all
        [HttpGet]
        [Route("children/all")]
        public List<Child> GetChildren()
        {
            return children;
        }

        private readonly DiscoveryHttpClientHandler _handler;
        private readonly IProjectService _projectService;

    }

}
