using Amazon.ElastiCacheCluster;
using Enyim.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Web;

namespace Tipstaff
{
    public class EasyCache
    {
        
        private static ObjectCache _cache;

        public EasyCache()
        {
            if (_cache == null)
            {
                _cache = MemoryCache.Default;
            }
        }

        public bool RefreshCache<T>(string key, T value, DateTimeOffset validFor)
        {
            return _cache.Add(key, value, validFor);
        }

        public T GetItem<T>(string key)
        {
            return (T)_cache.Get(key);
        }

        public IEnumerable<T> GetItems<T>(string key)
        {
            return (IEnumerable<T>)_cache.Get(key);
        }
     }
}