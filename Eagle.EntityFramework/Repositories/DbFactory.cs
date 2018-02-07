using Eagle.Core.Common;
using Eagle.Core.Configuration;

namespace Eagle.EntityFramework.Repositories
{
    public class DbFactory : DisposableObject, IDbFactory
    {
        private IDataContext _context;
        public DbFactory(IDataContext context)
        {
            _context = context;
        }

        public DbFactory()
        {

        }

        public IDataContext Init()
        {
            if (_context != null) return _context;

            _context = CreateObject(() => new DataContext(Settings.ConnectionString));

            return _context;
        }


        private bool _disposed;

        protected override void Dispose(bool isDisposing)
        {
            if (!this._disposed)
            {
                if (isDisposing)
                {
                    _context = null;
                }
                _disposed = true;
            }
            base.Dispose(isDisposing);
        }
    }
}
