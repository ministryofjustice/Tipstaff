using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace Tipstaff.Infrastructure.Cache
{
    public class Cache<T>
    {
        private static ObjectCache _cache;

        public Cache()
        {
            _cache = MemoryCache.Default;
        }

        public void Add(string key, T value, DateTimeOffset dateTimeOffset)
        {
            _cache.Add(key, value, dateTimeOffset);
        }

        public T GetItem(string key)
        {
            return (T)_cache.Get(key);
        }

        public IList<T> GetItems(string key)
        {
            return (IList<T>)_cache.Get(key);
        }

        public IDictionary<string,T> Getvalues(IList<string> keys)
        {
            return (IDictionary<string, T>)_cache.GetValues(keys);
        }

        public T AddOrGet(string key, T value, DateTimeOffset dateTimeOffset)
        {
            return  (T)_cache.AddOrGetExisting(key, value, dateTimeOffset);
        }
    }
}
