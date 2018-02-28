using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business
{

    public interface IAccountServices
    {
        bool Login(Account account);
        bool Validate(Account account);
    }

}
