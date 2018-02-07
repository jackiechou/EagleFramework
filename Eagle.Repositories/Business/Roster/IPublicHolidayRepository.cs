using System;
using Eagle.Entities.Business.Roster;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.Business.Roster
{
    public interface IPublicHolidayRepository: IRepositoryBase<PublicHoliday>
    {
        bool HasPublicHolidayExisted(DateTime holiday, int publicHolidaySetId);
    }
}
