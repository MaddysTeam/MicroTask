using Camoran.Redis.Utils;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Threading.Tasks;

namespace Camoran.Redis.PubSub
{

    public interface IRedisSubPub
    {
        void Publish<T>(string channel, T message);
        void Subscribe<T>(string channel, Action<string, T> cb);
        Task SubscribeAsync<T>(string channel, Action<string, T> cb);
        void UnSubscribe(string channel);
        void UnSubscribeAll();
        bool IsConnected { get; }
    }


    public class RedisSubPub : RedisBoss, IRedisSubPub
    {
        ISubscriber _subscriber;

        public RedisSubPub()
        {
            _subscriber = ConnectionManger.GetSubscriber();
        }

        public bool IsConnected => _subscriber.IsConnected();

        public void Publish<T>(string channel, T message)
        {
            var msg = JsonConvert.SerializeObject(message);
            _subscriber.Publish(channel, msg);
        }

        public void Subscribe<T>(string channel, Action<string, T> cb)
        {
            _subscriber.Subscribe(channel, (c, m) =>
            {
                var message = JsonConvert.DeserializeObject<T>(m);
                cb(c, message);
            });
        }

        public Task SubscribeAsync<T>(string channel, Action<string, T> cb)
        {
            return _subscriber.SubscribeAsync(channel, (c, m) =>
             {
                 var message = JsonConvert.DeserializeObject<T>(m);
                 cb(c, message);
             });
        }

        public void UnSubscribe(string channel)
        {
            _subscriber.Unsubscribe(channel);
        }

        public void UnSubscribeAll()
        {
            _subscriber.UnsubscribeAll();
        }
    }

}
