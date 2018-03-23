using Business;
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

        public AccountController(IConfiguration configuration, IDiscoveryClient client)
        {
            _config = configuration;
            _handler = new DiscoveryHttpClientHandler(client);
        }


        [Route("Login")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (model.AccessToken != null)
            {
                return Redirect(model.ReturnUrl);
            }

            var client = "client";
            var secret = "secret";
            var authority = "http://identity";
            var projectApi = "ProjectApi offline_access";
            var accessTokenResponse = await AuthService.RequestAccesstokenAsync(
                 new AuthTokenRequest(authority, client, secret, projectApi, "tom", "aaa", _handler),
                 AuthType.byResoucePassword);
            if (accessTokenResponse.IsSuccess)
            {
                Response.Cookies.Append("accessToken", accessTokenResponse.AccessToken);

                // 如果使用session
                //HttpContext.Session.SetString("accessToken", accessTokenResponse.AccessToken);
            }

            return Redirect(model.ReturnUrl);
        }

        [Route("Register")]
        public void Register()
        {

        }

        [Route("ChangePassword")]
        public void ChangePassword()
        {

        }

        [Route("ChangePassword")]
        public async Task LogOut()
        {

            //var user = HttpContext.User;

            //if (user.Identity.IsAuthenticated)
            //{
                    
            //}
            //TODO
            Response.Cookies.Delete("accessToken");

            // 如果使用session
            // HttpContext.Session.Remove("");
        }

        private readonly IConfiguration _config;
        private readonly DiscoveryHttpClientHandler _handler;

    }

}
