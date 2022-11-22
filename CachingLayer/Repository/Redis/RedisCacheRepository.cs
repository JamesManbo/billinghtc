using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace CachingLayer.Repository.Redis
{
    public class RedisCacheRepository : IRedisCacheRepository
    {
        private Lazy<ConnectionMultiplexer> _redisConnections;

        public RedisCacheRepository()
        {
        }

        public IDatabase RedisDatabase
        {
            get
            {
                if (this._redisConnections == null)
                {
                    InitializeConnection();
                }
                return _redisConnections?.Value.GetDatabase();
            }
        }

        public IServer RedisServer
        {
            get
            {
                if (this._redisConnections == null)
                {
                    InitializeConnection();
                }
                return _redisConnections?.Value.GetServer("redis:6379");
            }
        }

        private void InitializeConnection()
        {
            var option = new ConfigurationOptions
            {
                AbortOnConnectFail = false,
                ConnectTimeout = 1000,
                EndPoints = { "redis:6379" }
            };

            this._redisConnections = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(option));
        }

        #region IDisposable

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && _redisConnections != null)
            {
                _redisConnections?.Value.Close(true);
                _redisConnections?.Value.Dispose();
            }
        }

        # endregion
    }
}
