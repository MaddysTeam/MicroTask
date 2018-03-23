using IdentityModel;
using IdentityServer4.Validation;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Business
{

    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        IAccountServices _service;

        public ResourceOwnerPasswordValidator(IAccountServices service)
        {
            _service = service;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var user = new Account {Id="123", Name = context.UserName, Password = context.Password, Role = "admin" };

            //TODO: 调用redis cache 服务将用户放入缓存
            //registeduserList.Add(user);

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
