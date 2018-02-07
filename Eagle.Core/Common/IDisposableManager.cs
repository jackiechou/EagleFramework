using System;
using System.Collections.Generic;

namespace Eagle.Core.Common
{
    public interface IDisposableManager
    {
        IList<IDisposable> Disposables { get; }

        void Dispose(bool isDisposing);
    }
}
