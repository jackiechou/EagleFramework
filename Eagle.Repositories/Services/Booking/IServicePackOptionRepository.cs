using System.Collections.Generic;
using Eagle.Entities.Services.Booking;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.Services.Booking
{
    public interface IServicePackOptionRepository : IRepositoryBase<ServicePackOption>
    {
        IEnumerable<ServicePackOption> GetServicePackOptions(int packageId);
        bool HasDataExisted(int packageId, string optionName);
        int GetNewListOrder();
    }
}
