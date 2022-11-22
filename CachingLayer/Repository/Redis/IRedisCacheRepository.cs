using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace CachingLayer.Repository.Redis
{
    public interface IRedisCacheRepository
    {
        IDatabase RedisDatabase { get; }
        IServer RedisServer { get; }
    }
}
