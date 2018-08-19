using Amazon.ElastiCacheCluster;
using Enyim.Caching;
using Enyim.Caching.Memcached;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tipstaff.Infrastructure
{
    public class ElastiCacheImplementation 
    {
        public string InsertKeyValue(string value)
        {
            ElastiCacheClusterConfig elastiCacheClusterConfig = new ElastiCacheClusterConfig("test.gkak7w.cfg.euw2.cache.amazonaws.com", 11211);


            MemcachedClient memcachedClient = new MemcachedClient(elastiCacheClusterConfig);

            memcachedClient.Store(StoreMode.Set, "Demo", "Success");

            Object val;
            if (memcachedClient.TryGet("Demo", out val))
            {
                return val.ToString();
            }
            else
            {
                // Search Database if the get fails
                //Console.WriteLine("Checking database because get failed");
            }


            return "OOPS";

        }

        public string GetValue(string key)
        {
            ElastiCacheClusterConfig config = new ElastiCacheClusterConfig();

            MemcachedClient memClient = new MemcachedClient(config);


            var value = memClient.Get(key);

            return value.ToString();
        }
    }
}
