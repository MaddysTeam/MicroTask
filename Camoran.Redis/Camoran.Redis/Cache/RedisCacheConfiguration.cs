using System;
using System.Collections.Generic;
using System.Text;

namespace Camoran.Redis.Cache
{

    public class RedisCacheConfiguration
    {
        public string ConnectString { get; set; }
        public int DB { get; set; } = -1;
    }

}
