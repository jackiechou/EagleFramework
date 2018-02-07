using Eagle.Entities.Business.Roster;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.Business.Roster
{
    public interface IShiftPositionRepository: IRepositoryBase<ShiftPosition>
    {
        ShiftPosition GetDetails(int shiftId, int positionId);
    }
}
