using Business;
using Chloe.MySql;
using Common;
using DotNetCore.CAP;
using Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Steeltoe.Common.Discovery;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Controllers
{

   // [Route("project")]
    public class ProjectController : Controller
    {

        public ProjectController(
            IDiscoveryClient client,
            IProjectService projectService,
            MySqlContext context, 
            IConfiguration configuration,
            CAPDbContext dbContext, 
            ICapPublisher serviceBus
            )
        {
            _handler = new DiscoveryHttpClientHandler(client);
            _projectService = projectService;
            _dbContext = dbContext;
            _serviceBus = serviceBus;
            _config = configuration;
        }

        // POST project/edit
        [HttpPost]
        [Route("edit")]
        public void Edit([FromBody]Project project)
        {
            if (project.Id.IsNullOrEmpty())
            {
                _projectService.AddProject(project);
            }
        }


        // POST project/{id}
        [HttpGet]
        //[Route("{id}")]
        //[Authorize(Roles = "admin")]
        [Route("admin/{id}")]
        public Project GetProject(string id)
        {
            var project = _projectService.GetProjectById(id);

            return project;
        }


        [HttpGet]
        //[ActionLoggerFilter]
        public async Task<string> Get()
        {
            var client = _config.GetSection("Identity:Client").Value;
            var secret = _config.GetSection("Identity:Secret").Value;
            var authority = _config.GetSection("Identity:Authority").Value;
            var projectApi = _config.GetSection("Identity:Api").Value;

            var accessTokenResponse = await AuthService.RequestAccesstokenAsync(
                 new AuthTokenRequest(authority, client, secret, projectApi, "tom", "aaa", _handler),
                 AuthType.byResoucePassword);

            var httpClient = new HttpClient();
            httpClient.SetBearerToken(accessTokenResponse.AccessToken);

            var responseMessage = await httpClient.GetAsync("http://localhost:5555/project/admin");

            //return responseMessage;
            //throw new ProjectExcption()
            //var values = "aaaa";
            //HttpContext.Session.SetString("key", "strValue");
            //cache.Set("aaa", values);
            return string.Empty;
        }


        [Route("publish")]
        public void PublishMessage()
        {
            using (var trans = _dbContext.Database.BeginTransaction())
            {
                _serviceBus.Publish("xxx.project.check",
                new Business.Project
                {
                    Id = Guid.NewGuid().ToString(),
                    Code = "1001",
                    Name = "myPorject",
                    Owner = "owner"
                });

                trans.Commit();
            }
        }


        private readonly DiscoveryHttpClientHandler _handler;
        private readonly IProjectService _projectService;
        private readonly MySqlContext _sqlContext;
        private readonly IConfiguration _config;
        private readonly CAPDbContext _dbContext;
        private readonly ICapPublisher _serviceBus;

    }

}
