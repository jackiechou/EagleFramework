using System.Collections.Generic;
using System.Linq;
using Eagle.Core.Common;
using Eagle.Entities.Business.Roster;
using Eagle.EntityFramework;

namespace Eagle.Repositories.Business.Roster
{
    public class ShiftTypeRepository: RepositoryBase<ShiftType>, IShiftTypeRepository
    {
        public ShiftTypeRepository(IDataContext dataContext) : base(dataContext) { }
        public IEnumerable<ShiftType> GetByNetworkId(int? page, int? pageSize,
           ref int? recordCount)
        {
            var queryable = DataContext.Get<ShiftType>().Where(t => t.IsActive == true);

            if (recordCount != null)
            {
                recordCount = queryable.Count();
            }

            queryable = queryable.OrderByDescending(e => e.ShiftTypeId);

            if (page != null && pageSize != null)
            {
                queryable = queryable.ApplyPaging(page.Value, pageSize.Value);
            }

            return queryable.AsEnumerable();
        }


        public bool HasNameExisted(string timesheetShiftTypeName)
        {
            return DataContext.Get<ShiftType>()
                .Any(t => t.ShiftTypeName.ToLower() == timesheetShiftTypeName.ToLower());
        }


        public bool HasTimeOffTypeUsed(int timeOffTypeId, int? shiftTypeId)
        {
            return DataContext.Get<ShiftType>()
                .Any(t => t.TimeOffTypeId == timeOffTypeId && t.ShiftTypeId != shiftTypeId);
        }
    }
}
