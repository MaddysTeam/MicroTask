using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business
{

    public class AccountService : IAccountServices
    {

        public bool Login(Account account)
        {
            return true;
        }

        public bool Validate(Account account)
        {
            return true;
        }

    }

}
