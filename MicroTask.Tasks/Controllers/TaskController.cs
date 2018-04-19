using Chloe.MySql;
using DotNetCore.CAP;
using Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Steeltoe.Common.Discovery;

namespace Controllers
{

    [EnableCors("CORS")]
    public class TaskController : Controller
    {

        public TaskController(
            IDiscoveryClient client,
            MySqlContext choleContext,
            IConfiguration configuration,
            CAPDbContext capContext,
            ICapPublisher serviceBus
            )
        {
            _handler = new DiscoveryHttpClientHandler(client);
            _capContext = capContext;
            _serviceBus = serviceBus;
            _config = configuration;
            _choleContext = choleContext;
        }


        [HttpPost]
        [Authorize()]
        [Route("worktask/{id}")]
        public IActionResult GetTask(string id)
        {
            return Json(new { msg = "fuck" });
        }

        private readonly DiscoveryHttpClientHandler _handler;
        private readonly MySqlContext _choleContext;
        private readonly IConfiguration _config;
        private readonly CAPDbContext _capContext;
        private readonly ICapPublisher _serviceBus;

    }

}
