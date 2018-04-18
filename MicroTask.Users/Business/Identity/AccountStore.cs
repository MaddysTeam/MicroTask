using Chloe.MySql;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Business
{

    public class AccountStore :
        IUserStore<Account>,
        IUserPasswordStore<Account>
    {

        public AccountStore(MySqlContext context, LoggerFactory loggerfactory)
        {
            _context = context;
        }

        public Task<IdentityResult> CreateAsync(Account user, CancellationToken cancellationToken)
        {
            try
            {
                _context.Insert(() => new Account { Id = user.Id, PasswordHash = user.PasswordHash, Name = user.Name });
            }
            catch (Exception e)
            {
                return Task.FromResult(
                    IdentityResult.Failed(new IdentityError { Description = e.InnerException.Message })
                    );
            }

            return Task.FromResult(IdentityResult.Success);
        }

        public Task<IdentityResult> DeleteAsync(Account user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public void Dispose() { }

        public Task<Account> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            return Task.FromResult<Account>(null);
        }

        public Task<Account> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            var user = _context.Query<Account>()
                             .Select(x => new Account { Name = x.Name, PasswordHash = x.PasswordHash })
                             .Where(x => x.Name == normalizedUserName).FirstOrDefault();

            return Task.FromResult<Account>(user);
        }

        public Task<string> GetNormalizedUserNameAsync(Account user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetPasswordHashAsync(Account user, CancellationToken cancellationToken)
        {
            //查询数据库中 passwordHash
            //user = _context.Query<Account>()
            //                    .Select(x => new Account { Name = x.Name, PasswordHash = x.PasswordHash })
            //                    .Where(x => x.Name == user.Name).FirstOrDefault();

            return Task.FromResult(user.PasswordHash);
        }

        public Task<string> GetUserIdAsync(Account user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Id);
        }

        public Task<string> GetUserNameAsync(Account user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Name);
        }

        public Task<bool> HasPasswordAsync(Account user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetNormalizedUserNameAsync(Account user, string normalizedName, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.NormalizedUserName);
        }

        public Task SetPasswordHashAsync(Account user, string passwordHash, CancellationToken cancellationToken)
        {
            user.PasswordHash = passwordHash;

            return Task.FromResult(0);
        }

        public Task SetUserNameAsync(Account user, string userName, CancellationToken cancellationToken)
        {
            user.UserName = userName;

            return Task.FromResult(0);
        }

        public Task<IdentityResult> UpdateAsync(Account user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }


        MySqlContext _context;
        LoggerFactory _factory;
    }

}
