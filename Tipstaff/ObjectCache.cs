using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Web;

namespace Tipstaff
{
    public class MyObjectCache
    {
        static ObjectCache _cache = MemoryCache.Default;

        public static void Add(string key, object value)
        {
            _cache.Add(key, value, new CacheItemPolicy());
        }

        public static object Retrieve(string key)
        {
            return _cache.Get(key);
        }
    }
}