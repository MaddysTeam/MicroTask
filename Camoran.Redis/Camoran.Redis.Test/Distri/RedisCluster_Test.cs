using Camoran.Redis.Utils;
using Xunit;

namespace Camoran.Redis.Test
{

    public class RedisCluster_Test
    {

        [Fact]
        public void Redis_Cluster_Set_And_Change_Channel_Test()
        {
            RedisBoss.SetConnection("127.0.0.1:7000,127.0.0.1:7001");

            var rs = new RedisString();
            rs.Set("key", "value");

            RedisBoss.SetConnection("127.0.0.1:7003,127.0.0.1:7004");

            var val = rs.Get("key");
        }
    }

}
