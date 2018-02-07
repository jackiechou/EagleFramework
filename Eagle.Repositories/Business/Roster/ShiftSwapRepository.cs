using System.Collections.Generic;
using System.Linq;
using Eagle.Core.Common;
using Eagle.Core.Settings;
using Eagle.Entities.Business.Roster;
using Eagle.EntityFramework;

namespace Eagle.Repositories.Business.Roster
{
    public class ShiftSwapRepository : RepositoryBase<ShiftSwap>, IShiftSwapRepository
    {
        public ShiftSwapRepository(IDataContext dataContext) : base(dataContext) { }

        public IEnumerable<ShiftSwap> GetShiftSwaps(int? page, int? pageSize, ref int? recordCount)
        {
            var queryable = DataContext.Get<ShiftSwap>().Where(e => e.Status != ShiftSwapStatus.Rejected);

            if (recordCount != null)
            {
                recordCount = queryable.Count();
            }

            queryable = queryable.OrderByDescending(e => e.ShiftSwapId);

            if (page != null && pageSize != null)
            {
                queryable = queryable.ApplyPaging(page.Value, pageSize.Value);
            }

            return queryable.AsEnumerable();
        }
    }
}
