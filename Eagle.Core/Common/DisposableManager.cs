using System;
using System.Collections.Generic;

namespace Eagle.Core.Common
{
    public sealed class DisposableManager : DisposableObject, IDisposableManager
    {
        IList<IDisposable> IDisposableManager.Disposables
        {
            get { return Disposables; }
        }

        void IDisposableManager.Dispose(bool isDisposing)
        {
            Dispose(isDisposing);
        }
    }
}