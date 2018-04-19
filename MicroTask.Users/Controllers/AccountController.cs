using Business;
using Common;
using Common.Auth;
using Infrastructure;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Session;
using Microsoft.Extensions.Configuration;
using Steeltoe.Common.Discovery;
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
            _configuration = configuration;
            _handler = new DiscoveryHttpClientHandler(client);
            _signInManager = signInManager;
            _signInManager.UserManager = _userManager = userManager;
            _userManager.PasswordHasher = passwordHasher;
        }


        [Route("Login")]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Name, model.Password, false, false);
            if (result == AspNetCoreSingInResult.Success)
            {
                var accessToken = await GetAssessTokenByNameAndPassword(model.Name, model.Password);
                //Response.Cookies.Append("accessToken", accessToken);
                HttpContext.Session.SetString("accessToken", accessToken);
          
                if (!returnUrl.IsNullOrEmpty())
                    return Redirect(returnUrl);
                else
                {
                    dynamic account = new { model.Name, model.Password, accessToken };
                    return Ok(account);
                }
            }
            else if (result == AspNetCoreSingInResult.LockedOut)
                return new InternalServerErrorResult("账号锁定，请稍后重试");
            else
                return new InternalServerErrorResult("用户名或密码不正确");

        }

       
        [Route("Search")]
        public async Task<IActionResult> FindAccount(string username)
        {
            if(HttpContext.Session.GetString("accessToken").IsNullOrEmpty())
                return Json(new
                {
                    isSuccess = false,
                    msg = "您还未登陆"
                });
            var account = await _userManager.FindByNameAsync(username);
            if (!account.IsNull())
                return Json(new
                {
                    result = account,
                    isSuccess = true,
                    msg = "操作成功"
                });
            return Json(new
            {
                isSuccess = false,
                msg = "操作成功"
            });
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


        [HttpPost]
        [Route("ChangePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            var account = await _userManager.FindByNameAsync(model.Name);
            var result = await _userManager.ChangePasswordAsync(account, model.OldPassword, model.NewPassword);
            if (result.Succeeded)
                return Ok();
            else
                return new InternalServerErrorResult("更换密码失败");
        }

        [Route("LogOut")]
        public async Task LogOut()
        {

            // 如果使用session
            // HttpContext.Session.Remove("key");

            // 如果设置cookie
            // HttpContext.Response.Cookies.Delete("cookie");

            Response.Cookies.Delete("accessToken");
            HttpContext.Session.Remove("accessToken");
        }


        /// <summary>
        /// 获取accessToken,这里先写死一个服务
        /// </summary>
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private async Task<string> GetAssessTokenByNameAndPassword(string name, string password)
        {
            var client = _configuration.GetSection("Identity:Client").Value;
            var secret = _configuration.GetSection("Identity:Secret").Value;
            var authority = _configuration.GetSection("Identity:Authority").Value;
            var api = "TaskApi offline_access";
            var accessTokenResponse = await AuthService.RequestAccesstokenAsync(
                 new AuthTokenRequest(authority, client, secret, api, name, password, _handler),
                 AuthType.byResoucePassword);
            if (accessTokenResponse.IsSuccess)
            {
                return accessTokenResponse.AccessToken;
            }

            return string.Empty;
        }

        private readonly IConfiguration _configuration;
        private readonly DiscoveryHttpClientHandler _handler;
        private SignInManager<Account> _signInManager;
        private UserManager<Account> _userManager;

    }

}
