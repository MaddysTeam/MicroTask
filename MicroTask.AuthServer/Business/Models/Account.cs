using Microsoft.AspNetCore.Identity;

namespace Business
{

    public class Account: IdentityUser
    {
       public string Name { get; set; }
       public string Password { get; set; }
       public string Role { get; set; }
    }

    public class AccountRole:IdentityRole
    {
    }



    public class LoginViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string AccessToken { get; set; }
        public bool IsSuccess { get; set; }
        public string ReturnUrl { get; set; }
    }


    public class RegisterViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string ReturnUrl { get; set; }
    }

    public class ChangePasswordViewModel
    {
        public string Id { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
        public string ReturnUrl { get; set; }
    }

    //public class LoginOutViewModel { }


}
