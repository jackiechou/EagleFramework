using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using Eagle.Core.Common;
using Eagle.Entities;
using Eagle.EntityFramework;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories
{
    public abstract class Repository : DisposableObject, IRepository
    {
        public DateTime NullDateTime = DateTime.Parse("1/1/1900");

        protected Repository(IDataContext context)
        {
            DataContext = context;
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

        private bool disposed = false;

        protected override void Dispose(bool isDisposing)
        {
            if (!this.disposed)
            {
                if (isDisposing)
                {
                    DataContext = null;
                }
                disposed = true;
            }
            base.Dispose(isDisposing);
        }

    }
}
