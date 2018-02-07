using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Eagle.Core.Common;
using Eagle.EntityFramework.Repositories;
using Eagle.Repositories;
using log4net;

namespace Eagle.Services
{
    /// <summary>
    /// Base Service Implementation
    /// </summary>
    public abstract class BaseService : DisposableObject, IBaseService
    {
        public ILog Logger { get; set; }
        protected IUnitOfWork UnitOfWork { get; set; }
        protected bool OwnsUnitOfWork { get; private set; }
        private HashSet<IBaseService> Services { get; set; }
        private HashSet<BaseRepository> Repositories { get; set; }
        
        //static BaseService()
        //{
        //    MappingHelper.InitializeMapping();
        //}

        //protected BaseService(IUnitOfWork unitOfWork)
        //{
        //    if (unitOfWork == null) throw new ArgumentNullException(nameof(unitOfWork));
        //    UnitOfWork = unitOfWork;
        //}

        protected BaseService(IUnitOfWork unitOfWork = null, IEnumerable<IRepository> repositories = null, IEnumerable<IBaseService> sevices = null)
        {
            OwnsUnitOfWork = true;
            UnitOfWork = unitOfWork;
            Services = new HashSet<IBaseService>();
            Repositories = new HashSet<BaseRepository>();
            CurrentClaimsIdentity = ClaimsPrincipal.Current;

            //if (CurrentClaimsIdentity?.Claims == null || CurrentClaimsIdentity.Claims.Count() <= 1)
            //{
            //    HttpContext.Current.Response.Write(
            //        "<script type='text/javascript'>window.location = '/Admin/Login'</script>");
            //    HttpContext.Current.Response.Flush();
            //}

            if (sevices != null)
            {
                foreach (var service in sevices)
                {
                    RegisterService(service);
                }
            }

            if (repositories != null && UnitOfWork != null)
            {
                var repositoriesToRegister = repositories.OfType<BaseRepository>().ToList();
                UnitOfWork.RegisterRepositories<BaseRepository>(repositoriesToRegister);
                foreach (var repository in repositoriesToRegister)
                {
                    Repositories.Add(repository);
                }
            }
        }

        #region CLAIMS
        protected ClaimsPrincipal CurrentClaimsIdentity { get; set; }
        public Claim GetClaim(string type)
        {
            return ClaimsPrincipal.Current.Claims.FirstOrDefault(c => c.Type.ToString(CultureInfo.InvariantCulture) == type);
        }
        public void SetIdentity(ClaimsPrincipal identity)
        {
            CurrentClaimsIdentity = identity;
            if (Services.Any())
            {
                foreach (var service in Services)
                {
                    service.SetIdentity(identity);
                }
            }
        }
        public void SetClaim(string claim, string value)
        {
            SetClaim(new Claim(claim, value));
        }
        public void SetClaim(Claim claim)
        {
            if (CurrentClaimsIdentity.FindFirst(claim.Type) != null)
            {
                RemoveClaim(claim.Type);
            }
            var claims = new List<Claim> { claim };
            var identity = new ClaimsIdentity(claims);

            CurrentClaimsIdentity.AddIdentity(identity);
        }
        public void RemoveClaim(string claimType)
        {
            if (CurrentClaimsIdentity.FindFirst(claimType) != null)
            {
                var identity = CurrentClaimsIdentity.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    var claim = (from c in CurrentClaimsIdentity.Claims
                                 where c.Type == claimType
                                 select c).Single();
                    identity.RemoveClaim(claim);
                }
            }
        }
        #endregion

        protected virtual void Save(bool saveImmediately = false)
        {
            if (OwnsUnitOfWork || saveImmediately) UnitOfWork.SaveChanges();
        }

        protected virtual Task<int> SaveAsync()
        {
            return !OwnsUnitOfWork ? Task.FromResult(0) : UnitOfWork.SaveChangesAsync();
        }

        protected virtual Task<int> SaveAsync(CancellationToken cancellationToken)
        {
            return !OwnsUnitOfWork ? Task.FromResult(0) : UnitOfWork.SaveChangesAsync(cancellationToken);
        }

        protected TService GetService<TService>()
            where TService : class, IBaseService
        {
            var service = Services.FirstOrDefault(s => s is TService) as TService;
            return service;
        }

        protected void RegisterService<TService>(TService service)
            where TService : class, IBaseService
        {
            var baseService = service as BaseService;
            if (baseService != null)
            {
                baseService.OwnsUnitOfWork = false;
            }
            Services.Add(service);
        }

        protected void RegisterRepository<TRepository>(TRepository repository)
            where TRepository : BaseRepository, IRepository
        {
            Repositories.Add(repository);
        }

        private bool _disposed;

        protected override void Dispose(bool isDisposing)
        {
            if (!this._disposed)
            {
                if (isDisposing)
                {
                    UnitOfWork = null;
                    Services = null;
                    Repositories = null;
                    CurrentClaimsIdentity = null;
                }
                _disposed = true;
            }
            base.Dispose(isDisposing);
        }
    }
}
