using System.Linq;
using Eagle.Entities.Business.Roster;
using Eagle.EntityFramework;

namespace Eagle.Repositories.Business.Roster
{
    public class ShiftPositionRepository: RepositoryBase<ShiftPosition>, IShiftPositionRepository
    {
        public ShiftPositionRepository(IDataContext dataContext) : base(dataContext) { }
        public ShiftPosition GetDetails(int shiftId, int positionId)
        {
            return DataContext.Get<ShiftPosition>().FirstOrDefault(s => s.ShiftId == shiftId && s.PositionId == shiftId);
        }
    }
}
