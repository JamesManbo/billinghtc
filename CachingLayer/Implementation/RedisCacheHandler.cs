using CachingLayer.Repository.Redis;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CachingLayer.Implementation
{
    public class RedisCacheHandler : ICacheHandler
    {
        private readonly IRedisCacheRepository _redisCacheRepository;
        private readonly bool _isEnable;

        public RedisCacheHandler(IRedisCacheRepository redisCacheRepository, bool isEnable = true)
        {
            _redisCacheRepository = redisCacheRepository;
            _isEnable = isEnable;
        }

        public string[] FindKeys(string pattern)
        {
            return _redisCacheRepository.RedisServer
                .Keys(pattern: pattern)
                .Select(c => c.ToString()).ToArray();
        }

        public void Delete(string key)
        {
            _redisCacheRepository.RedisDatabase.KeyDelete(key);
        }

        public bool TryGet(out string data, string key)
        {
            data = null;
            if (this._isEnable && _redisCacheRepository.RedisDatabase.KeyExists(key))
            {
                var queryable = _redisCacheRepository.RedisDatabase.StringGet(key);
                if (queryable.IsNullOrEmpty) return false;

                data = queryable.ToString();
                return true;
            }

            return false;
        }

        public bool TryGet<T>(out T data, string key) where T : class
        {
            data = default(T);
            if (this._isEnable && _redisCacheRepository.RedisDatabase.KeyExists(key))
            {
                var queryable = _redisCacheRepository.RedisDatabase.StringGet(key);
                if (queryable.IsNullOrEmpty) return false;

                data = JsonConvert.DeserializeObject<T>(queryable.ToString(),
                    new JsonSerializerSettings()
                    {
                        TypeNameHandling = TypeNameHandling.Auto
                    });

                return true;
            }

            return false;
        }

        public void Set(string key, object data, long cacheTime)
        {
            if (data == null) return;

            var dataAsJSON = JsonConvert.SerializeObject(data,
            new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.Auto
            });

            _redisCacheRepository
                .RedisDatabase
                .StringSet(key, dataAsJSON, cacheTime > 0
                    ? (TimeSpan?)TimeSpan.FromSeconds(cacheTime)
                    : null);
        }
    }
}
