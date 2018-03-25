using System.Threading.Tasks;

namespace Business
{

    public interface IAccountServices
    {
        bool Login(Account account);
        bool Validate(Account account);
        Task<bool> LoginAsync(Account account);
        Task<bool> ValidateAsync(Account account);
        Task<Account> RegisterAsync(Account account);
        Task SignOutAsync();
    }

}
