using System;
using System.Collections.Generic;

namespace Eagle.Core.Common
{
    public abstract class DisposableObject : IDisposable
    {
        protected DisposableObject()
        {
            Disposables = new List<IDisposable>();
        }

        protected virtual void Dispose(bool isDisposing)
        {
            if (_isDisposed) return;

            _isDisposed = true;

            if (isDisposing)
            {
                foreach (var disposable in Disposables)
                {
                    disposable.Dispose();
                }
            }

            Disposables = null;
        }

        private void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);  
        }

        void IDisposable.Dispose()
        {
            Dispose();
        }

        private bool _isDisposed = false;

        protected IList<IDisposable> Disposables { get; private set; }

        protected T CreateObject<T>(Func<T> createObject) where T : IDisposable
        {
            var result = createObject();
            Manage(result);
            return result;
        }

        protected void Manage(IDisposable disposable)
        {
            if (disposable != null) Disposables.Add(disposable);
        }

        protected void Manage(IEnumerable<IDisposable> disposables)
        {
            foreach (var disposable in disposables)
            {
                Manage(disposable);
            }
        }
    }
}
