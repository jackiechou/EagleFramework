using System.Collections.Generic;
using System.Linq;
using Eagle.Core.Common;
using Eagle.Entities.Business.Roster;
using Eagle.EntityFramework;

namespace Eagle.Repositories.Business.Roster
{
    public class TimeOffTypeRepository : RepositoryBase<TimeOffType>, ITimeOffTypeRepository
    {
        public TimeOffTypeRepository(IDataContext dataContext) : base(dataContext) { }

        public IEnumerable<TimeOffType> GetTimeOffTypes(int? page, int? pageSize, ref int? recordCount)
        {
            var queryable = DataContext.Get<TimeOffType>().Where(e => e.IsActive == true);

            if (recordCount != null)
            {
                recordCount = queryable.Count();
            }

            queryable = queryable.OrderByDescending(e => e.TimeOffTypeId);

            if (page != null && pageSize != null)
            {
                queryable = queryable.ApplyPaging(page.Value, pageSize.Value);
            }

            return queryable.AsEnumerable();
        }

        public bool HasTimeOffTypeExisted(string name)
        {
            return DataContext.Get<TimeOffType>()
                .Any(r => r.TimeOffTypeName.ToLower() == name.ToLower() && r.IsActive == true);
        }
    }
}
