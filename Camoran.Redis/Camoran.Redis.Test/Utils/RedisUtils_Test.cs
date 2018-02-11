using Camoran.Redis.Utils;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Threading;
//using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Camoran.Redis.Test
{

    public class RedisUtils_Test
    {
        string conn = @"localhost";
        string defaultKey = "abc";
        string defaultValue = "bcd";
        TimeSpan defaultExpire = new TimeSpan(0, 0, 0, 1, 10);
        TimeSpan longExpire = new TimeSpan(1, 0, 0, 0, 0);
        RedisKeys rk = new RedisKeys();
        RedisString rs = new RedisString();
        RedisHash hs = new RedisHash();
        RedisList rl = new RedisList();
        RedisSet rst = new RedisSet();
        RedisSortedSet rss = new RedisSortedSet();


        public RedisUtils_Test()
        {
            RedisBoss.SetConnection(conn);
        }

        [Fact]
        public void Create_ConnectionManager_Test()
        {
            Assert.NotNull(GetManager());
        }

        [Fact]
        public void Connect_Test()
        {
            var cm = GetManager();

            Assert.True(cm.IsConnected);
        }


        #region [ Redis key ]

        [Fact]
        public void RedisKey_Key_Exists_Test()
        {
            rs.Set(defaultKey, defaultValue);

            Assert.True(rk.Exists(defaultKey));
        }

        [Fact]
        public void RedisKey_Key_Expire_Test()
        {
            rs.Set(defaultKey, defaultValue);

            rk.Expire(defaultKey, defaultExpire);

            Thread.Sleep(1000);

            Assert.True(!rk.Exists(defaultKey));
        }

        [Fact]
        public void RedisKey_Key_Persist_Test()
        {
            rs.Set(defaultKey, defaultValue);

            rk.Expire(defaultKey, defaultExpire);

            rk.Persist(defaultKey);

            Assert.True(rk.Exists(defaultKey));
        }

        [Fact]
        public void Redis_Key_Increment_Test()
        {
            var intValue="1";
            rs.Set(defaultKey, intValue);

            var current = rk.Increment(defaultKey, 10);

            Assert.Equal(current % 10, 1);
        }

        [Fact]
        public void Redis_Value_Dump_Test()
        {
            rs.Set(defaultKey, defaultValue);

            var val = rk.Doump(defaultKey);

            Assert.IsType<byte[]>(val);
        }

        [Fact]
        public void Redis_Value_Restore_Test()
        {
            rs.Set(defaultKey, defaultValue);

            var val = rk.Doump(defaultKey);

            rk.Expire(defaultKey, defaultExpire);
            Thread.Sleep(1000);
            rk.Restore(defaultKey, val, null);

            var v = rs.Get(defaultKey);

            Assert.Equal(v, defaultValue);
        }

        #endregion


        #region [ Redis String ]

        [Fact]
        public void RedisString_Set_Test()
        {
            rs.Set(defaultKey, defaultValue);
            var val = rs.Get(defaultKey);
            Assert.Equal(val, defaultValue);

            var newVal = "newVal";
            rs.Set(defaultKey, newVal);
            val = rs.Get(defaultKey);
            Assert.NotEqual(val, defaultValue);
            Assert.Equal(val, newVal);

            rs.Set(defaultKey, null);
            val = rs.Get(defaultKey);
            Assert.Equal(val, null);
        }

        [Fact]
        public void RedisString_Set_Object_With_Different_Reference()
        {
            var o = new Demo { Val = defaultValue };
            rs.Set(defaultKey, o);
            var o2 = rs.Get<Demo>(defaultKey);

            Assert.NotEqual(o, o2);
            Assert.Equal(o.Val, o2.Val);
        }

        #endregion


        #region [ Redis List ]

        [Fact]
        public void List_Push_Pop_Test()
        {
            var newKey = "newKey";
            var newValue = "newValue";
            var pushVal = rl.Lpush(newKey, newValue);
            var popVal = rl.Lpop(defaultKey);

            var temp = "temp";
            for (int i = 0; i <= 10; i++)
                rl.Rpush(temp, i.ToString());

            var rpopVal = rl.Rpop(temp);

           // Assert.Null(val);
            Assert.True(pushVal > 0);
            Assert.Equal(popVal, defaultValue);
            Assert.Equal(rpopVal, "10");
        }

        [Fact]
        public void List_Range_Test()
        {
            var key = "ddd";
            int start = 0, end = 10;

            for (int i = start; i <= end; i++)
                rl.Lrem(key, i.ToString(), 0);

            for (int i = start; i <= end; i++)
                rl.Rpush(key, i.ToString());

            var all = rl.Lrange(key, 0, -1);
            var range = rl.Lrange(key, 0, 2);

            Assert.Equal(all.Count, end + 1);
            Assert.Equal(range.Count, 3);

        }

        [Fact]
        public void List_Insert_Test()
        {
            var tempKey = "temp";
            var prev = "prev";
            var next = "next";

            rl.Lpush(tempKey, prev);
            rl.Linsert(tempKey, prev, next);

            var realPrev = rl.Lindex(tempKey, 0);
            var realNext = rl.Lindex(tempKey, 1);

            Assert.Equal(prev, realPrev);
            Assert.Equal(next, realNext);
        }

        #endregion


        #region [ Redis Set ]

        [Fact]
        public void Set_Add_Test()
        {
            rst.Sadd(defaultKey, defaultValue);
            rst.Sadd(defaultKey, defaultValue);

            var vals = rst.Smember(defaultKey);
            var count = rst.Scard(defaultKey);

            Assert.Equal(vals.Count, count);
        }

        [Fact]
        public void SMove_Test()
        {
            rst.Srem(defaultKey, defaultValue);
            rst.Sadd(defaultKey, defaultValue);

            var newKey = "newKey";
            var newVal = "newVal";

            var allVal = rst.Sadd(newKey, newVal);
            var moveVal = rst.Smove(defaultKey, newKey, defaultValue);

            var vals = rst.Smember(newKey);
            var count = rst.Scard(newKey);

            Assert.True(allVal && moveVal);
            Assert.Equal(vals.Count, count);
        }

        [Fact]
        public void SRemove_Test()
        {
            //rst.Srem(defaultKey, defaultValue);
            rst.Sadd(defaultKey, defaultValue);
            var remVal = rst.Srem(defaultKey, defaultValue);
            var vals = rst.Smember(defaultKey);
            var count = rst.Scard(defaultKey);

            Assert.True(remVal);
            Assert.Equal(vals.Count, count);
        }

        [Fact]
        public void SDiff_Test()
        {
            rst.Srem(defaultKey, defaultValue);
            rst.Sadd(defaultKey, defaultValue);
            rst.Sadd(defaultKey, "newVal3");
            rst.Sadd(defaultKey, "newVal4");

            var newKey = "xxx";
            rst.Sadd(newKey, defaultValue);
            rst.Sadd(newKey, "newVal1");
            rst.Sadd(newKey, "newVal2");

            var vals = rst.Sdiff(defaultKey, newKey);
            var count = rst.Scard(defaultKey);

            Assert.Equal(vals.Count, 2);
            Assert.True(vals.Contains("newVal3") && vals.Contains("newVal4"));
        }

        #endregion


        #region [ Redis SortSet ]

        [Fact]
        public void Zadd_Test()
        {
            rss.Zadd("zzz", "user", 1);
            rss.Zadd("zzz", "user", 2);
            rss.Zadd("zzz", "user", 3);

            var count = rss.Zcount("zzz");

            Assert.Equal(count, 1);
        }

        [Fact]
        public void ZRange_Test()
        {
            rss.Zadd("nick", "user1", 1);
            rss.Zadd("nick", "user2", 2);
            rss.Zadd("nick", "user3", 3);

            var rs1 = rss.ZrangeByRank("nick", 0, -1);
            var rs2 = rss.ZrangeByRank("nick", 0, 1);
            var rs3 = rss.ZrangeByRank("nick", 0, 2);

            Assert.Equal(rs1.Count, 3);
            Assert.Equal(rs2.Count, 2);
            Assert.Equal(rs1.Count, 3);
        }

        [Fact]
        public void ZRem_Test()
        {
            rss.Zadd("rem", "user1", 1);
            rss.Zadd("rem", "user2", 2);
            rss.Zrem("rem", "user1");
            var count = rss.Zcount("rem");
            var score = rss.ZScore("rem", "user2");
            Assert.Equal(count, 1);
            Assert.Equal(score, 2);
        }

        [Fact]
        public void ZRemRange_Test()
        {
            rss.Zadd("remRange", "user1", 1);
            rss.Zadd("remRange", "user2", 2);
            rss.Zadd("remRange", "user3", 3);
            rss.Zadd("remRange", "user4", 4);

            var removeCount = rss.ZremRangeByScore("remRange", 1, 1);
            var removeAll = rss.ZremRangeByScore("remRange", 1, 3);
            var score = rss.ZScore("remRange", "user4");

            Assert.Equal(removeCount, 1);
            Assert.Equal(removeAll, 2);
            Assert.NotNull(score);
            Assert.Equal(score, 4);
        }

        #endregion       


        #region [ Redis Lock ]

        [Fact]
        public void GetLock_With_Single_Thread_Test()
        {
            var rl = new RedisLock(rk, rs, defaultExpire); //test logic:expireDate is too small
            var r1 = rl.GetLock();
            var r2 = rl.GetLock();

            Assert.True(r1 && r2);
        }

        [Fact]
        public void GetLock_With_Multi_Threads_Test()
        {
            int threadCount = 10;
            var rl = new RedisLock(rk, rs, defaultExpire);
            var ls = new List<string>();
             var count = 0;

            rk.Del("lockObj");

            while (count <= 30)
            {
                ThreadPool.QueueUserWorkItem(s =>
                {
                    if (rl.GetLock())
                        ls.Add("got!");

                    count++;
                });
                //Task.Run(new Action(() =>
                //{
                //    if (rl.GetLock())
                //        ls.Add("got!");

                //    count++;
                //}));

                Thread.Sleep(10);
            }
            //for (int i = 0; i < threadCount; i++)
            //{
            //    new Thread(() =>
            //    {
            //        if (rl.GetLock())
            //            ls.Add("got!");
            //    }).Start();

            //    Thread.Sleep(5);
            //}

            Thread.Sleep(10000);

            Assert.True(ls.Count > 0);
        }

        #endregion


        private ConnectionMultiplexer GetManager()
        {
            RedisBoss.SetConnection(conn);

            return RedisBoss.ConnectionManger;
        }


        class Demo
        {
            public string Val { get; set; }
        }

    }

}
