//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.Linq;
//using System.Threading.Tasks;

//namespace Business.Identity.Models
//{

//    public class AccountOptions
//    {
//        public static bool AllowLocalLogin = true;
//        public static bool AllowRememberLogin = true;
//        public static TimeSpan RememberMeLoginDuration = TimeSpan.FromDays(30);

//        public static bool ShowLogoutPrompt = true;
//        public static bool AutomaticRedirectAfterSignOut = false;

//        // specify the Windows authentication scheme being used
//        public static readonly string WindowsAuthenticationSchemeName = Microsoft.AspNetCore.Server.IISIntegration.IISDefaults.AuthenticationScheme;
//        // if user uses windows auth, should we load the groups from windows
//        public static bool IncludeWindowsGroups = false;

//        public static string InvalidCredentialsErrorMessage = "Invalid username or password";
//    }


//    public class LoggedOutViewModel
//    {
//        public string PostLogoutRedirectUri { get; set; }
//        public string ClientName { get; set; }
//        public string SignOutIframeUrl { get; set; }

//        public bool AutomaticRedirectAfterSignOut { get; set; }

//        public string LogoutId { get; set; }
//        public bool TriggerExternalSignout => ExternalAuthenticationScheme != null;
//        public string ExternalAuthenticationScheme { get; set; }
//    }


//    public class LoginInputModel
//    {
//        [Required]
//        public string Username { get; set; }
//        [Required]
//        public string Password { get; set; }
//        public bool RememberLogin { get; set; }
//        public string ReturnUrl { get; set; }
//    }


//    public class LoginViewModel : LoginInputModel
//    {
//        public bool AllowRememberLogin { get; set; }
//        public bool EnableLocalLogin { get; set; }

//        public IEnumerable<ExternalProvider> ExternalProviders { get; set; }
//        public IEnumerable<ExternalProvider> VisibleExternalProviders => ExternalProviders.Where(x => !String.IsNullOrWhiteSpace(x.DisplayName));

//        public bool IsExternalLoginOnly => EnableLocalLogin == false && ExternalProviders?.Count() == 1;
//        public string ExternalLoginScheme => IsExternalLoginOnly ? ExternalProviders?.SingleOrDefault()?.AuthenticationScheme : null;
//    }


//    public class LogoutInputModel
//    {
//        public string LogoutId { get; set; }
//    }


//    public class LogoutViewModel : LogoutInputModel
//    {
//        public bool ShowLogoutPrompt { get; set; }
//    }

//}




//UserManager<ApplicationUser> userManager,
//          SignInManager<ApplicationUser> signInManager,
//            IIdentityServerInteractionService interaction,
//            IClientStore clientStore,
//            IAuthenticationSchemeProvider schemeProvider,
//            IEventService events)