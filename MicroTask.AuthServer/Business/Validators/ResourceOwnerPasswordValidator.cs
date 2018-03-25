using IdentityModel;
using IdentityServer4.Models;
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
            var user = new Account { Id = "123", Name = context.UserName, Password = context.Password, Role = "admin" };

            var isValid =await _service.ValidateAsync(user);

            if (isValid)
            {
                var claims = new List<Claim>() {
                new Claim("role",user.Role)
            };

                context.Result =
                    new GrantValidationResult(
                        user.Id,
                        OidcConstants.AuthenticationMethods.Password,
                        claims);
            }
            else
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidClient);
            }
        }

    }

}
