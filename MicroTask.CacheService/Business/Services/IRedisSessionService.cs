using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business
{

    public interface IRedisSessionService<Value>
    {
        Task<Value> GetSession(string key);
        Task SetSession(string key, Value value,TimeSpan expire);
        bool SessionIsExpire(string key);
    }

}
