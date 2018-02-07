using System;

namespace Eagle.EntityFramework.Repositories
{
    public interface IDbFactory : IDisposable
    {
        IDataContext Init();
    }
}
