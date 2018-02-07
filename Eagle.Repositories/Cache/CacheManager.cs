using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using Eagle.Core.Configuration;

namespace Eagle.Repositories.Cache
{
    public class CacheManager : ICacheManager
    {
        /// <summary>
        /// Cache object
        /// </summary>
        protected ObjectCache ObjectCache => MemoryCache.Default;
        /// <summary>
        /// Seconds to cache objects for by default
        /// </summary>
        private DateTimeOffset DefaultExpiryDateTime => DateTimeOffset.Now.AddSeconds(60 * GlobalSettings.DefaultCacheDuration); //20 minutes

        public void Add<T>(string key, T item)
        {
            Add(key, item, DefaultExpiryDateTime);
        }

        public void Add<T>(string key, T item, DateTimeOffset expires)
        {
            Add(key, item, new CacheItemPolicy { AbsoluteExpiration = expires });
        }

        public void Add<T>(string key, T item, TimeSpan slidingExpiration)
        {
            Add(key, item, new CacheItemPolicy { SlidingExpiration = slidingExpiration });
        }

        public void Add<T>(string key, T item, CacheItemPolicy policy)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            ValidateKey(key);

            ObjectCache.Set(key, item, policy);

            //_cache.AddOrUpdate(key, item, (newKey, newValue) => item);
        }

        /// <summary>
        /// Adds the specified key and object to the cache.
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="data">Data</param>
        /// <param name="cacheTime">Cache time</param>
        public void Set<T>(string key, object data, int cacheTime)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));
            ValidateKey(key);

            ObjectCache.Set(key, data, new CacheItemPolicy { AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(cacheTime) });
        }

        public bool IsSet<T>(string key)
        {
            return (ObjectCache.Contains(key));
        }

        /// <summary>
        /// Gets or sets the value associated with the specified key.
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="key">The key of the value to get.</param>
        /// <returns>The value associated with the specified key.</returns>
        public T Get<T>(string key)
        {
            ValidateKey(key);

            var item = ObjectCache[key];

            return UnwrapLazy<T>(item);
        }


        public async Task<T> GetAsync<T>(string key)
        {
            ValidateKey(key);

            var item = ObjectCache[key];

            return await UnwrapAsyncLazys<T>(item);
        }


        public T GetOrAdd<T>(string key, Func<T> addItemFactory)
        {
            return GetOrAdd(key, addItemFactory, DefaultExpiryDateTime);
        }


        public async Task<T> GetOrAddAsync<T>(string key, Func<Task<T>> addItemFactory, CacheItemPolicy policy)
        {
            ValidateKey(key);

            var newLazyCacheItem = new AsyncLazyCache<T>(addItemFactory);

            EnsureRemovedCallbackDoesNotReturnTheAsyncLazy<T>(policy);

            var existingCacheItem = ObjectCache.AddOrGetExisting(key, newLazyCacheItem, policy);

            if (existingCacheItem != null)
                return await UnwrapAsyncLazys<T>(existingCacheItem);

            try
            {
                var result = newLazyCacheItem.Value;

                if (result.IsCanceled || result.IsFaulted)
                    ObjectCache.Remove(key);

                return await result;
            }
            catch //addItemFactory errored so do not cache the exception
            {
                ObjectCache.Remove(key);
                throw;
            }
        }

        public T GetOrAdd<T>(string key, Func<T> addItemFactory, DateTimeOffset expires)
        {
            return GetOrAdd(key, addItemFactory, new CacheItemPolicy { AbsoluteExpiration = expires });
        }


        public T GetOrAdd<T>(string key, Func<T> addItemFactory, TimeSpan slidingExpiration)
        {
            return GetOrAdd(key, addItemFactory, new CacheItemPolicy { SlidingExpiration = slidingExpiration });
        }

        public T GetOrAdd<T>(string key, Func<T> addItemFactory, CacheItemPolicy policy)
        {
            ValidateKey(key);

            var newLazyCacheItem = new Lazy<T>(addItemFactory);

            EnsureRemovedCallbackDoesNotReturnTheLazy<T>(policy);

            var existingCacheItem = ObjectCache.AddOrGetExisting(key, newLazyCacheItem, policy);

            if (existingCacheItem != null)
                return UnwrapLazy<T>(existingCacheItem);

            try
            {
                return newLazyCacheItem.Value;
            }
            catch //addItemFactory errored so do not cache the exception
            {
                ObjectCache.Remove(key);
                throw;
            }
        }

        /// <summary>
        /// Removes the value with the specified key from the cache
        /// </summary>
        /// <param name="key">/key</param>
        public void Remove(string key)
        {
            ValidateKey(key);
            if (ObjectCache.Contains(key))
            {
                ObjectCache.Remove(key);
            }
        }

        /// <summary>
        /// Removes items by pattern
        /// </summary>
        /// <param name="pattern">pattern</param>
        public void RemoveByPattern(string pattern)
        {
            var regex = new Regex(pattern, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
            var keysToRemove = (from item in ObjectCache where regex.IsMatch(item.Key) select item.Key).ToList();

            foreach (string key in keysToRemove)
            {
                Remove(key);
            }
        }

        /// <summary>
        /// Clear all cache data
        /// </summary>
        public void Clear()
        {
            foreach (var item in ObjectCache)
                Remove(item.Key);
        }


        public bool Contains(string key)
        {
            return ObjectCache.Contains(key);
        }

        public int Count()
        {
            return ObjectCache.Count();
        }

        //public void RemoveSomeCaches(string keyStartwith)
        //{
        //    var myKeys = ObjectCache.Where(s => s.(keyStartwith)).ToList();
        //    if (myKeys.Any())
        //    {
        //        foreach (var key in myKeys)
        //        {
        //            ValidateKey(key);
        //            ObjectCache.Remove(key);
        //        }
        //    }
        //}

       

        public async Task<T> GetOrAddAsync<T>(string key, Func<Task<T>> addItemFactory)
        {
            return await GetOrAddAsync(key, addItemFactory, DefaultExpiryDateTime);
        }

        public async Task<T> GetOrAddAsync<T>(string key, Func<Task<T>> addItemFactory, DateTimeOffset expires)
        {
            return await GetOrAddAsync(key, addItemFactory, new CacheItemPolicy { AbsoluteExpiration = expires });
        }

        public async Task<T> GetOrAddAsync<T>(string key, Func<Task<T>> addItemFactory, TimeSpan slidingExpiration)
        {
            return await GetOrAddAsync(key, addItemFactory, new CacheItemPolicy { SlidingExpiration = slidingExpiration });
        }

        private static T UnwrapLazy<T>(object item)
        {
            var lazy = item as Lazy<T>;
            if (lazy != null)
                return lazy.Value;

            if (item is T)
                return (T)item;

            var asyncLazy = item as AsyncLazyCache<T>;
            if (asyncLazy != null)
                return asyncLazy.Value.Result;

            var task = item as Task<T>;
            if (task != null)
                return task.Result;

            return default(T);
        }

        private static async Task<T> UnwrapAsyncLazys<T>(object item)
        {
            var asyncLazy = item as AsyncLazyCache<T>;
            if (asyncLazy != null)
                return await asyncLazy.Value;

            var task = item as Task<T>;
            if (task != null)
                return await task;

            var lazy = item as Lazy<T>;
            if (lazy != null)
                return lazy.Value;

            if (item is T)
                return (T)item;

            return default(T);
        }

        private static void EnsureRemovedCallbackDoesNotReturnTheLazy<T>(CacheItemPolicy policy)
        {
            if (policy?.RemovedCallback != null)
            {
                var originallCallback = policy.RemovedCallback;
                policy.RemovedCallback = args =>
                {
                    //unwrap the cache item in a callback given one is specified
                    var item = args?.CacheItem?.Value as Lazy<T>;
                    if (item != null)
                        args.CacheItem.Value = item.IsValueCreated ? item.Value : default(T);
                    originallCallback(args);
                };
            }
        }

        private static void EnsureRemovedCallbackDoesNotReturnTheAsyncLazy<T>(CacheItemPolicy policy)
        {
            if (policy?.RemovedCallback != null)
            {
                var originallCallback = policy.RemovedCallback;
                policy.RemovedCallback = args =>
                {
                    //unwrap the cache item in a callback given one is specified
                    var item = args?.CacheItem?.Value as AsyncLazyCache<T>;
                    if (item != null)
                        args.CacheItem.Value = item.IsValueCreated ? item.Value : Task.FromResult(default(T));
                    originallCallback(args);
                };
            }
        }

        private void ValidateKey(string key)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentOutOfRangeException(nameof(key), "Cache keys cannot be empty or whitespace");
        }

        /// <summary>
        /// Check for item in cache
        /// </summary>
        /// <param name="key">Name of cached item</param>
        /// <returns></returns>
        public static bool Exists(string key)
        {
            return HttpContext.Current.Cache[key] != null;
        }
    }
}
