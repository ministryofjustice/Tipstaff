using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace Tipstaff.Cache
{
    public class EasyCache
    {
        private static ObjectCache _cache;

        public EasyCache()
        {
            _cache = MemoryCache.Default;
        }

        public bool RefreshCache<T>(string key, T value, DateTimeOffset dateTimeOffset)
        {
            return _cache.Add(key, value, dateTimeOffset);
        }

        public T GetItem<T>(string key)
        {
            return (T)_cache.Get(key);
        }

        public IEnumerable<T> GetItems<T>(string key)
        {
            return (IEnumerable<T>)_cache.Get(key);
        }

        public IDictionary<string,T> GetValues<T>(IList<string> keys)
        {
            return (IDictionary<string, T>)_cache.GetValues(keys);
        }

        public IEnumerable<T> AddOrGet<T>(string key, Func<IEnumerable<T>> func)
        {
            IEnumerable<T> dataset = default(IEnumerable<T>);
            dataset =  (IEnumerable<T>)_cache.AddOrGetExisting(key, null, new DateTimeOffset(DateTime.Now.AddMinutes(30)));
            if (dataset == null)
            {
                var results = func.Invoke();
                dataset = (IEnumerable<T>)_cache.AddOrGetExisting(key, results, new DateTimeOffset(DateTime.Now.AddMinutes(30)));
            }

            return dataset;
        }
    }
}
