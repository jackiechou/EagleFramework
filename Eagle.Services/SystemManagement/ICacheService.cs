using System.Runtime.Caching;

namespace Eagle.Services.SystemManagement
{
    public interface ICacheService : IBaseService
    {
        TData Get<TData>(string key) where TData : class;
        void Set(string key, object data);
        void Set(string key, object data, int cacheTime);
        bool IsSet(string key);
        void Add(string key, string value);
        void Add(string key, string value, CacheItemPolicy policy);
        void Remove(string key);
        void Clear();
        string GenerateKey(string prefix, params string[] keys);
    }
}
