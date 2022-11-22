using System;
using System.Collections.Generic;
using System.Text;

namespace CachingLayer.Implementation
{
    public interface ICacheHandler
    {
        string[] FindKeys(string patten);
        void Delete(string key);
        bool TryGet(out string data, string key);
        bool TryGet<T>(out T data, string key) where T : class;
        void Set(string key, object data, long cacheTime);
    }
}
