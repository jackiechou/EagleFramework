using System.Collections.Generic;
using Eagle.Core.Settings;
using Eagle.Entities.Business.Manufacturers;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.Business
{
   public interface IManufacturerRepository : IRepositoryBase<Manufacturer>
   {
       IEnumerable<Manufacturer> GetManufacturers(int vendorId, string searchText, ManufacturerStatus? status,
           ref int? recordCount, string orderBy = null, int? page = null, int? pageSize = null);
       Manufacturer GetDetails(int manufacturerId);
   }
}
