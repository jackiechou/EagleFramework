using System.Collections.Generic;

namespace Eagle.Common.Extensions
{
    public class DictionaryExtension<TKey, TValue> : Dictionary<TKey, TValue>
    {
        public new TValue this[TKey key]
        {
            get { return GetValue(key); }
            set { this[key] = value; }
        }

        public TValue GetValue(TKey key)
        {
            TValue value;
            if (TryGetValue(key, out value))
            {
                return value;
            }
            return default(TValue);
        }
    }
}
