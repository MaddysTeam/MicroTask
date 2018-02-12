using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business
{

    public interface IRedisCacheService<Key,Value>
    {
        bool SetCache(Key key,Value value);
        Value Get(Key key);
        bool Remove(Key key);
        bool IsExpire(Key key);
    }


    public interface IRedisCacheStringKeyValueService : IRedisCacheService<string,string> { }

}
