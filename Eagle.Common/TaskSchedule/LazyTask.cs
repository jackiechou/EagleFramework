using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Eagle.Common.TaskSchedule
{
    /// <summary>
    /// Implements a caching/provisioning model we can term LazyThreadSafetyMode.ExecutionAndPublicationWithoutFailureCaching
    /// - Ensures only a single provisioning attempt in progress
    /// - a successful result gets locked in
    /// - a failed result triggers replacement by the first caller through the gate to observe the failed state
    ///</summary>
    /// <remarks>
    /// Inspired by Stephen Toub http://blogs.msdn.com/b/pfxteam/archive/2011/01/15/asynclazy-lt-t-gt.aspx
    /// Implemented with sensible semantics by @LukeH via SO http://stackoverflow.com/a/33942013/11635
    /// </remarks>
    public sealed class LazyTask<T>
    {
        private readonly object _lock = new object();
        private readonly Func<Task<T>> _factory;
        private Task<T> _cached;

        public LazyTask(Func<Task<T>> factory)
        {
            if (factory == null) throw new ArgumentNullException("factory");
            _factory = factory;
        }

        /// <summary>
        /// Trigger a load attempt. If there is an attempt in progress, take that. If preceding attempt failed, trigger a retry.
        /// </summary>
        public Task<T> Value
        {
            get
            {
                lock (_lock)
                {
                    if ((_cached == null) || (_cached.IsCompleted && (_cached.Status != TaskStatus.RanToCompletion)))
                    {
                        _cached = Task.Run(_factory);
                    }
                    return _cached;
                }
            }
        }
    }
}
