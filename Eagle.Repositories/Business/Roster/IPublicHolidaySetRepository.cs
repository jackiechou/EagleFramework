using System;
using Eagle.Entities.Business.Roster;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.Business.Roster
{
    public interface IPublicHolidaySetRepository: IRepositoryBase<PublicHolidaySet>
    {
        int GetPublicHolidaySets();
        int GetPublicHolidaySets(DateTime startDate, DateTime endDate);
        bool HasPublicHolidaySetExisted(string description, int countryId);
    }
}
