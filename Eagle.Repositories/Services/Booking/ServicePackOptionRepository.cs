using System.Collections.Generic;
using System.Linq;
using Eagle.Entities.Services.Booking;
using Eagle.EntityFramework;

namespace Eagle.Repositories.Services.Booking
{
    public class ServicePackOptionRepository : RepositoryBase<ServicePackOption>, IServicePackOptionRepository
    {
        public ServicePackOptionRepository(IDataContext dataContext) : base(dataContext){ }

        public IEnumerable<ServicePackOption> GetServicePackOptions(int packageId)
        {
            return (from o in DataContext.Get<ServicePackOption>()
                    where o.PackageId == packageId
                    orderby o.ListOrder ascending
                    select o).AsEnumerable();
        }

        public bool HasDataExisted(int packageId, string optionName)
        {
            var query = (from o in DataContext.Get<ServicePackOption>()
                         where o.PackageId == packageId
                         && o.OptionName.ToLower() == optionName.ToLower()
                         select o).FirstOrDefault();
            return query != null;
        }

        public int GetNewListOrder()
        {
            int listOrder = 1;
            var query = from u in DataContext.Get<ServicePackOption>() select u.ListOrder;
            if (query.Any())
            {
                listOrder = query.Max() + 1;
            }
            return listOrder;
        }
    }
}
