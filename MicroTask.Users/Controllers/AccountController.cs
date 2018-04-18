using Business;
using Chloe.MySql;
using Common;
using DotNetCore.CAP;
using Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Steeltoe.Common.Discovery;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using AspNetCoreSingInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace Controllers
{

    [EnableCors("CORS")]
    public class AccountController : Controller
    {

        public AccountController(
            IConfiguration configuration,
            IDiscoveryClient client,
            SignInManager<Account> signInManager,
            UserManager<Account> userManager,
            IPasswordHasher<Account> passwordHasher
            )
        {
            _config = configuration;
            _handler = new DiscoveryHttpClientHandler(client);
            _signInManager = signInManager;
            _signInManager.UserManager = _userManager = userManager;
            _userManager.PasswordHasher = passwordHasher;
        }

        //[Authorize()]
        [Route("Test")]
        public string Test()
        {
            return string.Empty;
        }


        [Route("Login")]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Name, model.Password, false, false);
            if (result == AspNetCoreSingInResult.Success)
                if (!returnUrl.IsNullOrEmpty())
                    return Redirect(returnUrl);
                else
                {
                    dynamic account = new { model.Name, model.Password };
                    return Ok(account);
                }
            else if (result == AspNetCoreSingInResult.LockedOut)
                return new InternalServerErrorResult("账号锁定，请稍后重试");
            else
                return new InternalServerErrorResult("用户名或密码不正确");

        }


        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!model.Name.IsNullOrEmpty() && !model.Password.IsNullOrEmpty())
            {
                var account = new Account { Name = model.Name, Password = model.Password };
                var passwordHash = _userManager.PasswordHasher.HashPassword(account, account.Password);

                var result = await _userManager.CreateAsync(account);
                if (result.Succeeded)
                    return Redirect(model.ReturnUrl);

                return new InternalServerErrorResult("注册失败");
            }
            return new InternalServerErrorResult("验证失败");
        }



        //[Authorize()] TODO:这里Authorize()会导致 gateway 访问不到，估计和identity 冲突
        [HttpPost]
        [Route("ChangePassword")]
        public async void ChangePassword(ChangePasswordViewModel model)
        {
            var account = await _userManager.FindByNameAsync(model.Name);

            var result = await _userManager.ChangePasswordAsync(account, model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {

            }
            else
            {

            }
        }

        [Route("LogOut")]
        public async Task LogOut()
        {
            // 如果使用session
            // HttpContext.Session.Remove("key");

            // 如果设置cookie
            // HttpContext.Response.Cookies.Delete("cookie");
            // await _accountService.SignOutAsync();
        }

        private readonly IConfiguration _config;
        private readonly DiscoveryHttpClientHandler _handler;
        private SignInManager<Account> _signInManager;
        UserManager<Account> _userManager;

        //public AccountController(
        //    IDiscoveryClient client,
        //    IProjectService projectService,
        //    MySqlContext choleContext,
        //    IConfiguration configuration,
        //    CAPDbContext capContext,
        //    ICapPublisher serviceBus
        //    )
        //{
        //    _handler = new DiscoveryHttpClientHandler(client);
        //    _projectService = projectService;
        //    _capContext = capContext;
        //    _serviceBus = serviceBus;
        //    _config = configuration;
        //    _choleContext = choleContext;
        //}

        //// POST project/edit
        //[HttpPost]
        //[Route("edit")]
        //public void Edit([FromBody]Project project)
        //{
        //    if (project.Id.IsNullOrEmpty())
        //    {
        //        _projectService.AddProject(project);
        //    }

        //}


        //// POST project/{id}
        //[HttpGet]
        //[Authorize()]
        //[Route("project/{id}")]
        //public Project GetProject(string id)
        //{
        //    var project = _projectService.GetProjectById(id);

        //    return project;
        //}


        //[HttpGet]
        //[Route("admin/login")]
        //[TypeFilter(typeof(ActionLoggerFilter))]
        //public async Task<string> Get()
        //{
        //    //return responseMessage;
        //    //throw new ProjectExcption()
        //    //var values = "aaaa";
        //    HttpContext.Session.SetString("key", "strValue");

        //    return string.Empty;
        //    //cache.Set("aaa", values);            return string.Empty;
        //}


        //[Route("publish")]
        //public void PublishMessage()
        //{
        //    using (var trans = _capContext.Database.BeginTransaction())
        //    {
        //        _serviceBus.Publish("xxx.project.check",
        //        new Business.Project
        //        {
        //            Id = Guid.NewGuid().ToString(),
        //            Code = "1001",
        //            Name = "myPorject",
        //            Owner = "owner"
        //        });

        //        trans.Commit();
        //    }
        //}


        //[Route("CORSDemo")]
        //public ActionResult TryCORS()
        //{
        //    if (HttpContext.Session.GetString("test") == null)
        //        HttpContext.Session.SetString("test", "1111");
        //    Response.Cookies.Append("session_id", HttpContext.Session.Id);
        //    return Ok();
        //}


        //private readonly DiscoveryHttpClientHandler _handler;
        //private readonly IProjectService _projectService;
        //private readonly MySqlContext _choleContext;
        //private readonly IConfiguration _config;
        //private readonly CAPDbContext _capContext;
        //private readonly ICapPublisher _serviceBus;

    }

}
