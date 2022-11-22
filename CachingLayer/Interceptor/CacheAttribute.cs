using Autofac.Extras.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Text;

namespace CachingLayer.Interceptor
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class CacheAttribute : Attribute
    {
        public long Duration { get; set; }

        public CacheAttribute()
        {
        }
    }
}
