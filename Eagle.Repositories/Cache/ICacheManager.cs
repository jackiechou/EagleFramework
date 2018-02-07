using System;
using System.Runtime.Caching;
using System.Threading.Tasks;

namespace Eagle.Repositories.Cache
{
    /// <summary>
    /// Cache manager 
    /// </summary>
    public interface ICacheManager
    {
        void Add<T>(string key, T item);
        void Add<T>(string key, T item, DateTimeOffset absoluteExpiration);
        void Add<T>(string key, T item, TimeSpan slidingExpiration);
        void Add<T>(string key, T item, CacheItemPolicy policy);

        /// <summary>
        /// Adds the specified key and object to the cache.
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="data">Data</param>
        /// <param name="cacheTime">Cache time</param>
        void Set<T>(string key, object data, int cacheTime);

        /// <summary>
        /// Gets a value indicating whether the value associated with the specified key is cached
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>Result</returns>
        bool IsSet<T>(string key);

        /// <summary>
        /// Gets or sets the value associated with the specified key.
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="key">The key of the value to get.</param>
        /// <returns>The value associated with the specified key.</returns>
        T Get<T>(string key);

        T GetOrAdd<T>(string key, Func<T> addItemFactory);
        T GetOrAdd<T>(string key, Func<T> addItemFactory, DateTimeOffset absoluteExpiration);
        T GetOrAdd<T>(string key, Func<T> addItemFactory, TimeSpan slidingExpiration);
        T GetOrAdd<T>(string key, Func<T> addItemFactory, CacheItemPolicy policy);

        void Remove(string key);
        void RemoveByPattern(string pattern);
        void Clear();

        Task<T> GetOrAddAsync<T>(string key, Func<Task<T>> addItemFactory);
        Task<T> GetOrAddAsync<T>(string key, Func<Task<T>> addItemFactory, CacheItemPolicy policy);
        Task<T> GetOrAddAsync<T>(string key, Func<Task<T>> addItemFactory, DateTimeOffset expires);
        Task<T> GetOrAddAsync<T>(string key, Func<Task<T>> addItemFactory, TimeSpan slidingExpiration);
        Task<T> GetAsync<T>(string key);
    }
}
