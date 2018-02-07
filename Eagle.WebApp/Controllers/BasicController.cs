using System.Web.Mvc;

namespace Eagle.WebApp.Controllers
{
    public class BasicController : Controller
    {
        private bool _disposed;
        protected override void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                _disposed = true;
            }
            base.Dispose(_disposed);
        }
    }
}
