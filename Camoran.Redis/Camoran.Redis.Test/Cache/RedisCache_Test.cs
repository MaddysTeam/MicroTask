using Camoran.Redis.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Xunit;

namespace Camoran.Redis.Test
{

    public class RedisCache_Test
    {

        RedisEncryptKeyWithSetStrategy _rekwss = new RedisEncryptKeyWithSetStrategy(Encoding.UTF8);
        RedisEncryptKeyStrategy<List<string>> _reks = new RedisEncryptKeyStrategy<List<string>>(Encoding.UTF8);
        RedisEncryptKeyWithHashStrategy _rekwh = new RedisEncryptKeyWithHashStrategy((Encoding.UTF8));
        List<string> defaultValues = new List<string>();
        string defaultKey = "defaultKey";
        TimeSpan _defaultTimeSpan = new TimeSpan(0,0,0,0,1);

        public RedisCache_Test()
        {
            defaultValues.Add("val1");
            defaultValues.Add("val2");
            defaultValues.Add("val3");
        }


        [Fact]
        public void RedisEncryptKeyWithSetStrategy_Set_Get_Test()
        {
            _rekwss.Set(defaultKey, defaultValues);
            var vals = _rekwss.Get(defaultKey);

            Assert.Equal(vals.Count, defaultValues.Count);
        }


        [Fact]
        public void RedisEncryptKeyWithSetStrategy_Remove_Cache_Test()
        {
            _rekwss.Set(defaultKey, defaultValues);
            var vals = _rekwss.Get(defaultKey);
            _rekwss.Remove(defaultKey);
            var rvals = _rekwss.Get(defaultKey);

            Assert.Equal(vals.Count, defaultValues.Count);
            Assert.Equal(rvals.Count, 0);
        }


        [Fact]
        public void RedisEncryptKeyValueStrategy_Set_Get_Test()
        {
            _reks.Set(defaultKey, defaultValues);
            var vals = _reks.Get(defaultKey);

            Assert.Equal(vals.Count, defaultValues.Count);
        }

        [Fact]
        public void RedisEncryptHashValueStrategy_Set_Get_Test()
        {
            var dic = defaultValues.ToDictionary(x => x, y => y);
            _rekwh.Set(defaultKey, dic);
            var vals = _rekwh.Get(defaultKey);

            Assert.Equal(vals.Count, defaultValues.Count);
        }


        [Fact]
        public void RedisEncryptHashValueStrategy_Key_Expire_Test()
        {
            var dic = defaultValues.ToDictionary(x => x, y => y);
            _rekwh.Set(defaultKey, dic, _defaultTimeSpan);
            Thread.Sleep(1000);
            var rs = _rekwh.Get(defaultKey);

            Assert.Null(rs);
        }

    }

}
