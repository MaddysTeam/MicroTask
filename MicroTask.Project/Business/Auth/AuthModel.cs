using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bussiess.Auth
{
    public class AuthModel
    {
       public string Authority { get; set; }
       public string Client { get; set; }
       public string Secret { get; set; }
       public string AccessToken { get; set; }
       public bool IsSuccess { get; set; }
       public string Error { get; set; }
    }
}
