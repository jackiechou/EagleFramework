using System;
using System.Linq;
using Eagle.Entities.Business.Roster;
using Eagle.EntityFramework;

namespace Eagle.Repositories.Business.Roster
{
    public class PublicHolidayRepository : RepositoryBase<PublicHoliday>, IPublicHolidayRepository
    {
        public PublicHolidayRepository(IDataContext dataContext) : base(dataContext) { }
        public bool HasPublicHolidayExisted(DateTime holiday, int publicHolidaySetId)
        {
            return DataContext.Get<PublicHoliday>().Any(
                s => s.Holiday == holiday && s.PublicHolidaySetId == publicHolidaySetId);
        }
    }
}
