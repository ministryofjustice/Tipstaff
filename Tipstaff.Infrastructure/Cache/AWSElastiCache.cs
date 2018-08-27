using Amazon.ElastiCacheCluster;
using Enyim.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tipstaff
{
    public class AWSElastiCache
    {
        private static ElastiCacheClusterConfig _elastiCacheClusterConfig;
        private static MemcachedClient _client;

        public object CacheItem { get; set; }

        public AWSElastiCache()
        {
            _elastiCacheClusterConfig = new ElastiCacheClusterConfig("main.gkak7w.cfg.euw2.cache.amazonaws.com", 11211);
            _client = new MemcachedClient(_elastiCacheClusterConfig);
            _client.Store(Enyim.Caching.Memcached.StoreMode.Add, "Key","Value");
        }

        public static bool Add(string key, object value, TimeSpan validFor)
        {
            return _client.Store(Enyim.Caching.Memcached.StoreMode.Add, key, value, validFor);
        }

        public static bool Set(string key, object value, TimeSpan validFor)
        {
            return _client.Store(Enyim.Caching.Memcached.StoreMode.Set, key, value, validFor);
        }

        public static bool Remove(string key)
        {
            return _client.Remove(key);
        }

        public static T GetItem<T>(string key)
        {
           return (T)_client.Get(key);
        }

        public static IEnumerable<T> GetItems<T>(string key)
        {
            return (IEnumerable<T>)_client.Get(key);
        }
        
        public bool TryGet(string key, out object value)
        {
            return _client.TryGet(key, out value);
        }
        

        public void FlushAll()
        {
            _client.FlushAll();
        }
    }
}