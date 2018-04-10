using Business;
using Common;
using Common.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Steeltoe.Common.Discovery;
using System.Threading.Tasks;

namespace Controllers
{

    /// <summary>
    /// 账户控制器
    /// 用于登录，注册和修改密码
    /// 思考
    /// </summary>
    [Route("Account")]
    public class AccountController : Controller
    {

        public AccountController(
            IConfiguration configuration,
            IDiscoveryClient client,
            IAccountServices service
            )
        {
            _config = configuration;
            _handler = new DiscoveryHttpClientHandler(client);
            _accountService = service;
        }


        [Route("Login")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var isSuccess =await _accountService.LoginAsync(new Account { UserName=model.Name, Password=model.Password, Name=model.Name  });
            if (isSuccess)
            {
                // 如果使用session
                //HttpContext.Session.SetString("key", "value");

                if (!model.ReturnUrl.IsNullOrEmpty())
                {
                    return Redirect(model.ReturnUrl);
                }

                return Ok();
            }

            else
            {
                return Forbid();//TODO
            }
        }

        [Route("Register")]
        public void Register(RegisterViewModel model)
        {

        }

        [Route("ChangePassword")]
        public void ChangePassword(ChangePasswordViewModel model)
        {

        }

        [Route("LogOut")]
        public async Task LogOut()
        {
           await _accountService.SignOutAsync();
        }

        [Route("AccessToken")]
        public async Task<string> GetAssessTokenByNameAndPassword(string name, string password)
        {
            var client = "client";
            var secret = "secret";
            var authority = "http://identity";
            var projectApi = "ProjectApi offline_access";
            var accessTokenResponse = await AuthService.RequestAccesstokenAsync(
                 new AuthTokenRequest(authority, client, secret, projectApi, name, password, _handler),
                 AuthType.byResoucePassword);
            if (accessTokenResponse.IsSuccess)
            {
                return accessTokenResponse.AccessToken;
            }

            return string.Empty;
        }

        private readonly IConfiguration _config;
        private readonly DiscoveryHttpClientHandler _handler;
        private readonly IAccountServices _accountService;

    }

}
