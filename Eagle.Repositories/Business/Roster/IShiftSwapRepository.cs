using System.Collections.Generic;
using Eagle.Entities.Business.Roster;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.Business.Roster
{
    public interface IShiftSwapRepository: IRepositoryBase<ShiftSwap>
    {
        IEnumerable<ShiftSwap> GetShiftSwaps(int? page, int? pageSize, ref int? recordCount);
    }
}
