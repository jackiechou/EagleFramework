using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Eagle.Core.Common;
using Eagle.Core.Settings;
using Eagle.Entities.Business.Roster;
using Eagle.EntityFramework;

namespace Eagle.Repositories.Business.Roster
{
    public class TimesheetRepository: RepositoryBase<Timesheet>, ITimesheetRepository
    {
        public TimesheetRepository(IDataContext dataContext) : base(dataContext) { }
      
        public IEnumerable<Timesheet> GetAll(int? page, int? pageSize, ref int? recordCount)
        {
            var queryable = DataContext.Get<Timesheet>().Where(e => e.Status != TimesheetStatus.Cancelled);

            if (recordCount != null)
            {
                recordCount = queryable.Count();
            }

            queryable = queryable.OrderByDescending(e => e.TimesheetId);

            if (page != null && pageSize != null)
            {
                queryable = queryable.ApplyPaging(page.Value, pageSize.Value);
            }

            return queryable.AsEnumerable();
        }
        public IEnumerable<Timesheet> GetTimesheets(DateTime startDate, DateTime endDate)
        {
            return DataContext.Get<Timesheet>().Where(w => DbFunctions.TruncateTime(w.StartTime) >= DbFunctions.TruncateTime(startDate.Date)
                && DbFunctions.TruncateTime(w.EndTime) <= DbFunctions.TruncateTime(endDate.Date) && w.Status != TimesheetStatus.Cancelled);
        }
        public IEnumerable<Timesheet> GetTimesheetsByTimesheetIds(IEnumerable<int> timesheetIds)
        {
            var result = DataContext.Get<Timesheet>().Where(e => timesheetIds.Contains(e.TimesheetId) && (e.Status != TimesheetStatus.Cancelled));
            return result;
        }
        public Timesheet GetByShiftId(int shiftId)
        {
            var result = DataContext.Get<Timesheet>().FirstOrDefault(e => e.ShiftId == shiftId && e.Status != TimesheetStatus.Cancelled);
            return result;
        }

        public bool HasShiftExisted(int shiftId, int? timesheetId = null)
        {
            if (DataContext.Get<Timesheet>().Any(t => t.ShiftId == shiftId && t.Status != TimesheetStatus.Cancelled && t.TimesheetId != timesheetId))
                return true;
            return false;
        }

        public bool HasEmployeeTimeOffExisted(int employeeTimeOffId, int? timesheetId = null)
        {
            if (DataContext.Get<Timesheet>().Any(t => t.EmployeeTimeOffId == employeeTimeOffId && t.Status != TimesheetStatus.Cancelled && t.TimesheetId != timesheetId))
                return true;
            return false;
        }
    }
}
