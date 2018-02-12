using Common;
using IdentityModel;
using IdentityServer4.Validation;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Business
{

    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {

        static List<Account> registeduserList;

        public ResourceOwnerPasswordValidator()
        {
            if (registeduserList.IsNull())
            {
                registeduserList = new List<Account>();
            }
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var user = new Account {  Name=context.UserName, Password= context.Password };

            //TODO: 调用redis cache 服务将用户放入缓存
            registeduserList.Add(user);

            var claims = new List<Claim>() {
                new Claim("role",user.Role)
            };

            context.Result = 
                new GrantValidationResult(
                    user.Id,
                    OidcConstants.AuthenticationMethods.Password, 
                    claims);
        }

    }

}


//调用用户中心的验证用户名密码接口
//var client = new HttpClient(_handler);
//var url = $"http://{UserApplicationName}/search?name={context.UserName}&password={context.Password}";
//var result = await client.GetAsync(url);
//if (result.IsSuccessStatusCode)
//{
//    var user = await result.Content.ReadAsObjectAsync<dynamic>();
//    var claims = new List<Claim>() { new Claim("role", user.role.ToString()) };
//    var subject = user.id.ToString();
//    context.Result = new GrantValidationResult(subject, OidcConstants.AuthenticationMethods.Password, claims);
//}