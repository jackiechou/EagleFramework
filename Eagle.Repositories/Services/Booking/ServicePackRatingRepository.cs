using System.Collections.Generic;
using System.Linq;
using Eagle.Entities.Services.Booking;
using Eagle.EntityFramework;

namespace Eagle.Repositories.Services.Booking
{
    public class ServicePackRatingRepository : RepositoryBase<ServicePackRating>, IServicePackRatingRepository
    {
        public ServicePackRatingRepository(IDataContext dataContext) : base(dataContext) { }
        public IEnumerable<ServicePackRating> GetServicePackRatings(int servicePackId)
        {
            return DataContext.Get<ServicePackRating>().Where(x => x.PackageId == servicePackId);
        }
    }
}
