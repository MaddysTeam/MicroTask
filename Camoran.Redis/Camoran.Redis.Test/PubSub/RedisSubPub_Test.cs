using Camoran.Redis.PubSub;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Xunit;

namespace Camoran.Redis.Test
{

    public class RedisSubPub_Test
    {
        IRedisSubPub _subPub;
        string _defaultChannel = "myChannel";

        public RedisSubPub_Test()
        {
            _subPub = new RedisSubPub();
        }

        [Fact]
        public void Subscribe_String_Message_Test()
        {
            var index = 0;
            _subPub.Subscribe<string>(_defaultChannel, (c, m) => {
                index++;
                Assert.Equal(index,1);
                Assert.Equal(m, "Hello");
                Thread.Sleep(1000);
            });

            _subPub.Publish(_defaultChannel, "Hello");
            Thread.Sleep(1000);
        }

        [Fact]
        public void Subscribe_Message_Test()
        {
            _subPub.Subscribe<Message>(_defaultChannel, (c, m) => {
                Assert.NotNull(m);
                Assert.Equal(m.Body, "Hello");
                Thread.Sleep(1000);
            });

            _subPub.Publish(_defaultChannel, new Message { Body = "Hello" });

            Assert.True(_subPub.IsConnected);
            Thread.Sleep(1000);
        }

        class Message
        {
           public string Body { get; set; }
        }
    }

}
