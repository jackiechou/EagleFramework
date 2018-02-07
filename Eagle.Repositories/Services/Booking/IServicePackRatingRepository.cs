using System.Collections.Generic;
using Eagle.Entities.Services.Booking;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.Services.Booking
{
    public interface IServicePackRatingRepository : IRepositoryBase<ServicePackRating>
    {
        IEnumerable<ServicePackRating> GetServicePackRatings(int servicePackId);
    }
}
