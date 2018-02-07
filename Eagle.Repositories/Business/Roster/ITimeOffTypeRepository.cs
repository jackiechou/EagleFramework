using System.Collections.Generic;
using Eagle.Entities.Business.Roster;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.Business.Roster
{
    public interface ITimeOffTypeRepository : IRepositoryBase<TimeOffType>
    {
        IEnumerable<TimeOffType> GetTimeOffTypes(int? page, int? pageSize, ref int? recordCount);
        bool HasTimeOffTypeExisted(string name);
    }
}
