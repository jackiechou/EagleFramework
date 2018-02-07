using System;
using System.Globalization;
using System.Security.Claims;
using Eagle.Core.Common;
using Eagle.EntityFramework;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories
{
    /// <summary>
    /// Base Repository.
    /// </summary>
    public abstract class BaseRepository : DisposableObject, IRepository
    {
        public DateTime NullDateTime = DateTime.Parse("1/1/1900");

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseRepository"/> class.
        /// </summary>
        /// <param name="dataContext">The platform context.</param>
        /// <remarks>
        /// Concrete repositories should inject more specific interfaces that inherit from IPlatformContext.
        /// </remarks>
        protected BaseRepository(IDataContext dataContext)
        {
            DataContext = dataContext;
            InstanceId = Guid.NewGuid();

            CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");
            DateTimeFormatInfo dateformat = new DateTimeFormatInfo
            {
                ShortDatePattern = "MM/dd/yyyy"
            };
            culture.DateTimeFormat = dateformat;
            System.Threading.Thread.CurrentThread.CurrentCulture = culture;
        }
        protected ClaimsPrincipal CurrentClaimsIdentity { get; private set; }
        public void SetIdentity(ClaimsPrincipal identity)
        {
            CurrentClaimsIdentity = identity;
        }

        /// <summary>
        /// Gets or sets the platform context.
        /// </summary>
        /// <value>
        /// The platform context.
        /// </value>
        /// <remarks>
        /// This property should be visible to UnitOfWork to enable cross PlatformContexts transaction management.
        /// </remarks>
        protected internal IDataContext DataContext { get; set; }

        private Guid InstanceId { get; set; }

        Guid IRepository.InstanceId
        {
            get { return InstanceId; }
        }

        private bool _disposed = false;

        protected override void Dispose(bool isDisposing)
        {
            if (!this._disposed)
            {
                if (isDisposing)
                {
                    DataContext = null;
                }
                _disposed = true;
            }
            base.Dispose(isDisposing);
        }
    }
}
