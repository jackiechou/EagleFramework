using System;
using System.Linq;
using Eagle.Entities.Business.Roster;
using Eagle.EntityFramework;

namespace Eagle.Repositories.Business.Roster
{
    public class PublicHolidaySetRepository : RepositoryBase<PublicHolidaySet>, IPublicHolidaySetRepository
    {
        public PublicHolidaySetRepository(IDataContext dataContext) : base(dataContext) { }

        public int GetPublicHolidaySets()
        {
            return DataContext.Get<PublicHolidaySet>().Count();
        }

        public int GetPublicHolidaySets(DateTime startDate, DateTime endDate)
        {
            return DataContext.Get<PublicHolidaySet>().Count();
        }
     
        public bool HasPublicHolidaySetExisted(string description, int countryId)
        {
            return DataContext.Get<PublicHolidaySet>().Any(
                s => s.Description.ToLower().Trim().Equals(description.ToLower().Trim()) &&
                    s.CountryId == countryId);
        }
    }
}
