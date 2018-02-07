using System;
using System.Collections.Generic;
using System.Linq;
using Eagle.Entities.SystemManagement;
using Eagle.EntityFramework;

namespace Eagle.Repositories.SystemManagement
{
    public class UserAddressRepository : RepositoryBase<UserAddress>, IUserAddressRepository
    {
        public UserAddressRepository(IDataContext dataContext) : base(dataContext) { }
        public IEnumerable<UserAddressInfo> GetList(Guid userId)
        {
            return (from u in DataContext.Get<UserAddress>()
                    join a in DataContext.Get<Address>() on u.AddressId equals a.AddressId into addressInfo
                    from address in addressInfo.DefaultIfEmpty()
                    join c in DataContext.Get<Country>() on address.CountryId equals c.CountryId into countryJoin
                    from country in countryJoin.DefaultIfEmpty()
                    join p in DataContext.Get<Province>() on address.ProvinceId equals p.ProvinceId into provinceJoin
                    from province in provinceJoin.DefaultIfEmpty()
                    join r in DataContext.Get<Region>() on address.RegionId equals r.RegionId into regionJoin
                    from region in regionJoin.DefaultIfEmpty()
                    where u.UserId == userId
                    select new UserAddressInfo
                    {
                        UserAddressId= u.UserAddressId,
                        AddressId = u.AddressId,
                        UserId = u.UserId,
                        IsDefault = u.IsDefault,
                        Address = address,
                        Country= country,
                        Province = province,
                        Region = region
                    }).AsEnumerable();
        }

        public UserAddress GetDetails(Guid userId, int addressId)
        {
            return (from p in DataContext.Get<UserAddress>()
                where p.UserId == userId && p.AddressId == addressId
                select p).FirstOrDefault();
        }
        
        public bool HasDataExisted(Guid userId, int addressId)
        {
            var query = from p in DataContext.Get<UserAddress>()
                        where p.UserId == userId && p.AddressId == addressId
                        select p;
            return query.Any();
        }
    }
}
