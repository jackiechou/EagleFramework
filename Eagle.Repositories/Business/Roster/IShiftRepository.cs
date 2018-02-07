using System;
using System.Collections.Generic;
using Eagle.Entities.Business.Roster;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.Business.Roster
{
    public interface IShiftRepository : IRepositoryBase<Shift>
    {
        IEnumerable<Shift> GetIntersectEmployeeShifts(int employeeId, int? shiftId, DateTime startTime, DateTime endTime);
        float? GetTotalHoursOfEmployeeInWeek(int employeeId, DateTime startDate);
        bool HasFourConsetiveSundays(DateTime shiftDate);
    }
}
