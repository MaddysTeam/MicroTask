namespace Business
{

    public class Account
    {
       public string Id { get; set; }
       public string Name { get; set; }
       public string Password { get; set; }
       public string Role { get; set; }
    }


    public class LoginViewModel
    {
        public string Id { get; set; }
        public string Password { get; set; }
        public string AccessToken { get; set; }
        public bool IsSuccess { get; set; }
        public string ReturnUrl { get; set; }
    }

    //public class LoginOutViewModel { }


}
