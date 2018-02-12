using Camoran.Redis.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.Text.Encodings.Web;

namespace Camoran.Redis.Cache
{

    public class RedisEncrypt
    {
        protected EncryptType _entryptType;
        protected Encoding _encoding = Encoding.UTF8;
        protected HMACMD5 _md5;

        public RedisEncrypt(EncryptType entryptType)
        {
            _entryptType = entryptType;
            _md5 = new HMACMD5();
        }

        protected string MD5Encrypt(string key)
        {
            var buffer = _encoding.GetBytes(key);
            _md5.Key = buffer;
            var encryptKeyBytes = _md5.ComputeHash(buffer);
            var encryptKey = Encoding.ASCII.GetString(encryptKeyBytes);
            return encryptKey;
        }

        protected string SHAEncrypt(string key)
        {
            throw new RedisCacheException();
        }


        public enum EncryptType
        {
            MD5 = 1
        }
    }


    public class RedisEncryptKeyStrategy<Value> : RedisEncrypt, ICahceStrategy<string, Value>
    {
        RedisString _redisString;
        RedisKeys _redisKey;

        public RedisEncryptKeyStrategy(EncryptType entryptType = EncryptType.MD5)
            : base(entryptType)
        {
            _redisString = new RedisString();
            _redisKey = new RedisKeys();
        }

        public Value Get(string key)
        {
            var enctryptKey = MD5Encrypt(key);

            return _redisString.Get<Value>(enctryptKey);
        }

        public void Set(string key, Value val, TimeSpan? expireTime = null)
        {
            if (val == null || string.IsNullOrEmpty(key)) return;
            if (_entryptType == EncryptType.MD5)
            {
                var enctryptKey = MD5Encrypt(key);

                _redisString.Set(enctryptKey, val);

                if (expireTime != null)
                    SetExpire(key, expireTime.Value);
            }
        }

        public bool Remove(string key)
        {
            return _redisKey.Del(MD5Encrypt(key));
        }

        public void SetExpire(string key, TimeSpan expireTime)
        {
            _redisKey.Expire(key, expireTime);
        }
    }


    public class RedisEncryptKeyWithSetStrategy : RedisEncrypt, ICahceStrategy<string, List<string>>
    {
        RedisSet _set;
        RedisKeys _key;

        public RedisEncryptKeyWithSetStrategy(EncryptType entryptType = EncryptType.MD5)
            : base(entryptType)
        {
            _set = new RedisSet();
            _key = new RedisKeys();
        }

        public List<string> Get(string key)
        {
            var enctryptKey = MD5Encrypt(key);

            return _set.Smember(enctryptKey);
        }

        public void Set(string key, List<string> val, TimeSpan? expireTime = null)
        {
            if (val == null) return;
            if (_entryptType == EncryptType.MD5)
            {
                var enctryptKey = MD5Encrypt(key);

                foreach (var i in val)
                    _set.Sadd(enctryptKey, i); // redis set will remove duplicate values

                if (expireTime != null)
                    SetExpire(key, expireTime.Value);
            }
        }

        public bool Remove(string key)
        {
            return _key.Del(MD5Encrypt(key));
        }

        public void SetExpire(string key, TimeSpan expireTime)
        {
            _key.Expire(key, expireTime);
        }

    }


    public class RedisEncryptKeyWithHashStrategy : RedisEncrypt, ICahceStrategy<string, Dictionary<string, string>>
    {
        RedisHash _hash;
        RedisKeys _key;

        public RedisEncryptKeyWithHashStrategy(EncryptType entryptType = EncryptType.MD5)
            : base(entryptType)
        {
            _hash = new RedisHash();
            _key = new RedisKeys();
        }

        public Dictionary<string, string> Get(string key)
            => _hash.HGetAll(MD5Encrypt(key));


        public bool Remove(string key)
        {
            return _key.Del(MD5Encrypt(key));
        }

        public void Set(string key, Dictionary<string, string> val, TimeSpan? expireTime = null)
        {
            if (val == null) return;
            if (_entryptType == EncryptType.MD5)
            {
                var enctryptKey = MD5Encrypt(key);
                _hash.Hset(enctryptKey, val);

                if (expireTime != null)
                    SetExpire(enctryptKey, expireTime.Value);
            }
        }

        public void SetExpire(string key, TimeSpan expireTime)
            => _key.Expire(key, expireTime);
    }

}

