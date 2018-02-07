using System.Collections.Generic;
using Eagle.Entities.Business.Roster;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.Business.Roster
{
    public interface IShiftTypeRepository: IRepositoryBase<ShiftType>
    {
        IEnumerable<ShiftType> GetByNetworkId(int? page, int? pageSize,
            ref int? recordCount);

        bool HasNameExisted(string timesheetShiftTypeName);
        bool HasTimeOffTypeUsed(int timeOffTypeId, int? shiftTypeId);
    }
}
