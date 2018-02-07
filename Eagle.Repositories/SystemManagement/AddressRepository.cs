using System.Linq;
using Eagle.Entities.SystemManagement;
using Eagle.EntityFramework;

namespace Eagle.Repositories.SystemManagement
{
    public class AddressRepository : RepositoryBase<Address>, IAddressRepository
    {
        public AddressRepository(IDataContext dataContext) : base(dataContext)
        {
        }

        public AddressInfo GetDetails(int addressId)
        {
            var address = (from a in DataContext.Get<Address>()
                         join c in DataContext.Get<Country>() on a.CountryId equals c.CountryId into acJoin
                         from country in acJoin.DefaultIfEmpty()
                         join p in DataContext.Get<Province>() on a.ProvinceId equals p.ProvinceId into apJoin
                         from province in apJoin.DefaultIfEmpty()
                         join r in DataContext.Get<Region>() on a.RegionId equals r.RegionId into arJoin
                         from region in arJoin.DefaultIfEmpty()
                         where a.AddressId == addressId
                         select new AddressInfo
                         {
                             AddressId = a.AddressId,
                             AddressTypeId = a.AddressTypeId,
                             Street = a.Street,
                             PostalCode = a.PostalCode,
                             Description = a.Description,
                             CreatedDate = a.CreatedDate,
                             ModifiedDate = a.ModifiedDate,
                             CountryId =  a.CountryId,
                             ProvinceId =  a.ProvinceId,
                             RegionId =  a.RegionId,
                             Country = country,
                             Province = province,
                             Region = region
                         }).FirstOrDefault();
            return address;
        }

    }
}
