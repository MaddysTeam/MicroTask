using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Business
{

    public class AccountIdentityService : IAccountServices
    {

        public AccountIdentityService(
            UserManager<Account> userManager,
            SignInManager<Account> signInManager,
            ILogger logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        public bool Login(Account account)
        {
            throw new System.NotImplementedException();
        }

        public async Task<bool> LoginAsync(Account account)
        {
            var result = await _signInManager.PasswordSignInAsync(account, account.Password, false, false);

            return result.Succeeded;
        }

        public bool Validate(Account account)
        {
            return true;
        }

        public async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<bool> ValidateAsync(Account account)
        {
            return true;
        }

        public async Task<Account> RegisterAsync(Account account)
        {
            var result = await _userManager.CreateAsync(account);

            throw new System.NotImplementedException();
        }

        private readonly UserManager<Account> _userManager;
        private readonly SignInManager<Account> _signInManager;
        private readonly ILogger _logger;

    }

}
