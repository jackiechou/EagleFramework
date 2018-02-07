using System.Collections.Generic;
using System.Linq;
using Eagle.Core.Common;
using Eagle.Core.Settings;
using Eagle.Entities.Business.Manufacturers;
using Eagle.EntityFramework;

namespace Eagle.Repositories.Business
{
    public class ManufacturerRepository: RepositoryBase<Manufacturer>, IManufacturerRepository
    {
        public ManufacturerRepository(IDataContext dataContext) : base(dataContext) { }
        public IEnumerable<Manufacturer> GetManufacturers(int vendorId, string searchText, ManufacturerStatus? status, ref int? recordCount, string orderBy = null, int? page = null, int? pageSize = null)
        {
            var lst = (from m in DataContext.Get<Manufacturer>()
                       where m.VendorId == vendorId && (status == null || m.IsActive == status) || m.ManufacturerName.Contains(searchText)
                       select m);
            return lst.WithRecordCount(ref recordCount).WithSortingAndPaging(orderBy, page, pageSize);
        }

        public Manufacturer GetDetails(int manufacturerId)
        {
            return (from m in DataContext.Get<Manufacturer>()
                    where m.ManufacturerId == manufacturerId
                    select m).FirstOrDefault();
        }
    }
}
