using IdentityModel;
using IdentityServer4.Test;
using IdentityServer4.Validation;
using MicroTask.IdentityAuthServer;
using Steeltoe.Common.Discovery;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Business
{

    /// <summary>
    ///自定义验证
    ///step 1: 得在Startup.cs中 ConfigureService中调用
    ///step 2: 主要功能是每当客户端（亦或是其他服务）访问IdentityServer自动触发自定验证功能
    ///step 3: 进入ValidateAsync方法，返回验证结果
    ///step 4: 建议验证在本服务进行（server 作为Account中心），从而避免和用户模块耦合
    /// </summary>
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {

        private const string UserApplicationName = "";

        /// <summary>
        /// 触发自定义验证方法
        /// </summary>
        /// <param name="context">验证上下文</param>
        /// <returns></returns>
        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            // demo 

            var claims = new List<Claim>();
            var user = Config.GetUsers().Find(x => x.Password == context.Password && x.Username == context.UserName);
            if (user != null)
            {
                claims.Add(new Claim("userName", user.Username));
                context.Result = new GrantValidationResult(user.SubjectId, OidcConstants.AuthenticationMethods.Password, claims);
            }
            else
            {
                context.Result = new GrantValidationResult();
            }
        }

    }

}
