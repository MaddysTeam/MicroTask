namespace Business
{

    public interface IAccountServices
    {
        bool Login(Account account);
        bool Validate(Account account);
    }

}
