using Business;
using Chloe.MySql;
using Common;
using Domain;
using DotNetCore.CAP;
using Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Steeltoe.Common.Discovery;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Controllers
{

    [EnableCors("CORS")]
    public class ProjectController : Controller
    {

        public ProjectController(
            IDiscoveryClient client,
            IProjectService projectService,
            MySqlContext choleContext,
            IConfiguration configuration,
            CAPDbContext capContext,
            ICapPublisher serviceBus
            )
        {
            _handler = new DiscoveryHttpClientHandler(client);
            _projectService = projectService;
            _capContext = capContext;
            _serviceBus = serviceBus;
            _config = configuration;
            _choleContext = choleContext;
        }


        // POST project/{id}

        [HttpGet]
        //[Authorize()]
        [Route("{id}")]
        public Project GetProject(string id)
        {
            var project = _projectService.GetProjectById(id);

            return project;
        }

        // POST project/edit

        [HttpPost]
        [Route("edit")]
        public IActionResult Edit([FromBody]Project project)
        {
            if (project == null) return Json(new { });

            var isExist = !_projectService.GetProjectById(project.Id).IsNull();
            if (!isExist)
            {
                _projectService.AddProject(project);
                return Json(new { });
            }
            else 
            {
                _projectService.UpdateProject(project);
                return Json(new { });
            }
        }


        [Authorize]
        [HttpPost]
        [Route("delete")]
        public IActionResult DeleteProject(string id)
        {
            if (string.IsNullOrEmpty(id)) return Json(new { });
            var isExist = !_projectService.GetProjectById(id).IsNull();
            if (!isExist)
            {
                _projectService.RemoveProject(id);
                return Json(new { });
            }
            else
            {
                return Json(new { });
            }

            //    HttpContext.Session.SetString("key", "strValue");
            //    return string.Empty;
            //    //cache.Set("aaa", values);            return string.Empty;
        }


        [Route("publish")]
        public void PublishMessage()
        {
            using (var trans = _capContext.Database.BeginTransaction())
            {
                _serviceBus.Publish("xxx.project.check",
                new Domain.ProjectCreatedEvent
                {
                    //Id = Guid.NewGuid().ToString(),
                    //Code = "1001",
                    //Name = "myPorject",
                    //OwnerId = "owner"
                });

                trans.Commit();
            }
        }


        [Route("CORSDemo")]
        public ActionResult TryCORS()
        {
            if (HttpContext.Session.GetString("test") == null)
                HttpContext.Session.SetString("test", "1111");
            Response.Cookies.Append("session_id", HttpContext.Session.Id);
            return Ok();
        }


        private readonly DiscoveryHttpClientHandler _handler;
        private readonly IProjectService _projectService;
        private readonly MySqlContext _choleContext;
        private readonly IConfiguration _config;
        private readonly CAPDbContext _capContext;
        private readonly ICapPublisher _serviceBus;

    }

}
