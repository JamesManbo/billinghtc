using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationUserIdentity.API.Models.Configs
{
    public class AppSettings
    {
        public bool CacheEnable { get; set; }
        public string MD5CryptoKey { get; set; }
        public string CachingRedisServer { get; set; }
        public string CachingRedisServerPassword { get; set; }
    }
}
