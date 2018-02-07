using System;
using System.Runtime.Caching;
using System.Web.Caching;
using Eagle.Repositories;
using CacheItemPriority = System.Runtime.Caching.CacheItemPriority;

namespace Eagle.Services.SystemManagement
{
    public class CacheService : BaseService, ICacheService
    {
        public CacheService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        /// <summary>
        /// Fetches the specified entity from the cache.
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public TData Get<TData>(string key) where TData : class
        {
            try
            {
                return UnitOfWork.CacheManager.Get<TData>(key);
            }
            catch
            {
                return null;
            }
        }

        public void Set(string key, object data)
        {
            UnitOfWork.CacheManager.Add(key, data, new CacheItemPolicy { AbsoluteExpiration = Cache.NoAbsoluteExpiration.ToUniversalTime(), Priority = CacheItemPriority.Default });
        }

        public void Set(string key, object data, int cacheTime)
        {
            UnitOfWork.CacheManager.Add(key, data, new CacheItemPolicy { AbsoluteExpiration = DateTime.UtcNow + TimeSpan.FromMinutes(cacheTime) });
        }
        public bool IsSet(string key)
        {
            return UnitOfWork.CacheManager.IsSet<string>(key);
        }

        public void Add(string key, string value)
        {
            UnitOfWork.CacheManager.Add(key, value, new CacheItemPolicy {AbsoluteExpiration = Cache.NoAbsoluteExpiration.ToUniversalTime(),Priority = CacheItemPriority.Default});
        }
        public void Add(string key, string value, CacheItemPolicy policy)
        {
            UnitOfWork.CacheManager.Add(key, value, policy);
        }
        public void Remove(string key)
        {
            UnitOfWork.CacheManager.Remove(key);
        }

        public void Clear()
        {
            UnitOfWork.CacheManager.Clear();
        }
        public string GenerateKey(string prefix, params string[] keys)
        {
            return $"{prefix}-{string.Join("-", keys)}";
        }
    }
}
