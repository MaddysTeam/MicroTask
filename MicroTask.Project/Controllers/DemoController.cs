using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Business;
using DotNetCore.CAP;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Steeltoe.Common.Discovery;

namespace MicroTask.Controllers
{

    public class DemoController : Controller
    {
        //static List<Child> children;
        //static List<Parent> parents;

        //public DemoController(IDiscoveryClient client,IProjectService projectService)
        //{
        //    _handler = new DiscoveryHttpClientHandler(client);
        //    _projectService = projectService;

        //    if (children == null)
        //        children = new List<Child>();
        //    if (parents == null)
        //        parents = new List<Parent>();
        //}

        //// GET api/values
        //[HttpGet]
        //[Route("parent/demo")]
        //public async Task<IEnumerable<string>> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// POST parent/regist
        //[HttpPost]
        //[Route("parent/regist")]
        //public void RegistParent([FromBody]Parent parent)
        //{
        //    if (parent != null)
        //        parents.Add(parent);
        //}

        //// POST child/regist
        //[HttpPost]
        //[Route("child/regist")]
        //public void RegistChild([FromBody]Child child)
        //{
        //    if (child != null)
        //        children.Add(child);
        //}

        //// POST child/id
        //[HttpPost]
        //[Route("child/{id}")]
        //public Child GetChild(int id)

        //{
        //    return children.Find(x => x.Id == id.ToString());
        //}

        //// POST children/all
        //[HttpGet]
        //[Route("children/all")]
        //public List<Child> GetChildren()
        //{
        //    return children;
        //}

        //private readonly DiscoveryHttpClientHandler _handler;
        //private readonly IProjectService _projectService;

        [Route("publish/demo")]
        public async void PublishMessage([FromServices]ICapPublisher publisher)
        {
            await publisher.PublishAsync("xxx.project.check",
                new Business.Project
                {
                    Id = Guid.NewGuid().ToString(),
                    Code = "1001",
                    Name = "myPorject",
                    Owner = "owner"
                });
        }

        [CapSubscribe("xxx.project.check")]
        public async Task SubscribeMessage(Business.Project project)
        {
            Console.WriteLine(project.Name);
        }

    }

}
