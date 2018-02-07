using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Eagle.Common.TaskSchedule
{
    /// <summary>
    /// Provides support for lazy initialization in asyncronous manner.
    /// </summary>
    /// <typeparam name="T">Specifies the type of object that is being lazily initialized.</typeparam>
    public sealed class AsyncLazy<T>
    {
        #region Fields

        private object _syncObject = new object();
        private Task<T> _initializeTask;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncLazy"/> class.
        /// When lazy initialization occurs, the specified initialization function is used.
        /// </summary>
        /// <param name="valueFactory">The delegate that is invoked to produce the lazily initialized value when
        /// it is needed.</param>
        public AsyncLazy(Func<T> valueFactory)
        {
            if (valueFactory == null)
            {
                throw new ArgumentNullException("valueFactory");
            }
            _initializeTask = new Task<T>(valueFactory);
        }

        #endregion

        /// <summary>
        ///  Gets the lazily initialized value of the current instance.
        ///  If value have not been initialized, block calling thread until value get initialized.
        ///  If during initialization exception have been thrown, it will wrapped into <see cref="AggregateException"/>
        ///  and rethrowned on accessing this property.
        /// </summary>
        public T Value
        {
            get
            {
                if (_initializeTask.Status == TaskStatus.Created)
                {
                    lock (_syncObject)
                    {
                        if (_initializeTask.Status == TaskStatus.Created)
                        {
                            _initializeTask.RunSynchronously();
                        }
                    }
                }

                return _initializeTask.Result;
            }
        }

        /// <summary>
        /// Gets a value that indicates whether a value has been created for this instance.
        /// </summary>
        /// <value>
        ///     <c>true</c> if a value has been created; otherwise, <c>false</c>.
        /// </value>
        public bool IsValueCreated
        {
            get
            {
                return _initializeTask.IsCompleted;
            }
        }

        /// <summary>
        /// Initializes value on background thread.
        /// Calling thread will never be blocked.
        /// </summary>
        public void InitializeAsync()
        {
            if (_initializeTask.Status == TaskStatus.Created)
            {
                lock (_syncObject)
                {
                    if (_initializeTask.Status == TaskStatus.Created)
                    {
                        _initializeTask.Start();
                    }
                }
            }
        }
    }

    //How to use https://16handles.wordpress.com/2011/04/21/asynchronous-lazy-initialization/
    //private static AsyncLazy<HeavyObject> _heavyObject = new AsyncLazy<HeavyObject>(ObjectFactory);

    //static void Main(string[] args)
    //{
    //    Console.WriteLine("Doing some work");

    //    Console.WriteLine("It's time to begin initialize heavy object!");
    //    _heavyObject.InitializeAsync();

    //    Console.WriteLine("Accessing heavy object");
    //    HeavyObject heavyObject = _heavyObject.Value;
    //    heavyObject.SayHello();
    //}

    ///// <summary>
    ///// AsyncLazy<T> that derives from Lazy<Task<T>> and provides two constructors. 
    ///// </summary>
    ///// <typeparam name="T"></typeparam>
    //public class AsyncLazy<T> : Lazy<Task<T>>
    //{
    //    public AsyncLazy(Func<T> valueFactory) : 
    //        base(() => Task.Factory.StartNew(valueFactory)) { }

    //    public AsyncLazy(Func<Task<T>> taskFactory) : 
    //        base(() => Task.Factory.StartNew(() => taskFactory()).Unwrap()) { }

    //    public TaskAwaiter<T> GetAwaiter() { return Value.GetAwaiter(); }
    //}

    //How to use
    //Basic Method 01
    //static string LoadString() { … }
    //static AsyncLazy<string> m_data = new AsyncLazy<string>(LoadString);
    //string data = await m_data.Value;

    //Method Use with delegate
    //static AsyncLazy<string> m_data = new AsyncLazy<string>(async delegate
    //{
    //    WebClient client = new WebClient();
    //    return (await client.DownloadStringTaskAsync(someUrl)).ToUpper();
    //});
}
