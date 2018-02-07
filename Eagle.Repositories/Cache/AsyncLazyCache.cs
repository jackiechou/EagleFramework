using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Eagle.Repositories.Cache
{
    /// <summary>
    ///     See https://blogs.msdn.microsoft.com/pfxteam/2011/01/15/asynclazyt/
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AsyncLazyCache<T> : Lazy<Task<T>>
    {
        public AsyncLazyCache(Func<T> valueFactory) :
            base(() => Task.Factory.StartNew(valueFactory))
        {
        }

        public AsyncLazyCache(Func<Task<T>> taskFactory) :
            base(() => Task.Factory.StartNew(taskFactory).Unwrap())
        {
        }
        public TaskAwaiter<T> GetAwaiter() { return Value.GetAwaiter(); }
    }
}
