using System;
using System.Collections.Generic;
using Eagle.Entities.Business.Roster;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.Business.Roster
{
    public interface ITimesheetRepository: IRepositoryBase<Timesheet>
    {
        IEnumerable<Timesheet> GetAll(int? page, int? pageSize, ref int? recordCount);
        IEnumerable<Timesheet> GetTimesheets(DateTime startDate, DateTime endDate);
        IEnumerable<Timesheet> GetTimesheetsByTimesheetIds(IEnumerable<int> timesheetIds);
        Timesheet GetByShiftId(int shiftId);
        bool HasShiftExisted(int shiftId, int? timesheetId = null);
        bool HasEmployeeTimeOffExisted(int employeeTimeOffId, int? timesheetId = null);

    }
}
