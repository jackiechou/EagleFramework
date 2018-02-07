using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Eagle.Entities.Business.Roster;
using Eagle.EntityFramework;

namespace Eagle.Repositories.Business.Roster
{
    public class ShiftRepository: RepositoryBase<Shift>, IShiftRepository
    {
        public ShiftRepository(IDataContext dataContext) : base(dataContext) { }
        public IEnumerable<Shift> GetIntersectEmployeeShifts(int employeeId, int? shiftId, DateTime startTime, DateTime endTime)
        {
            var result = DataContext.Get<Shift>().Where(s => s.EmployeeId == employeeId && s.ShiftId != shiftId
                && ((s.StartTime >= startTime && s.StartTime <= endTime) || (s.EndTime >= startTime && s.EndTime <= endTime)));
            return result.AsEnumerable();
        }
        public float? GetTotalHoursOfEmployeeInWeek(int employeeId, DateTime startDate)
        {
            DateTime startDateOfWeek = startDate.AddDays((DayOfWeek.Monday) - startDate.DayOfWeek);
            startDateOfWeek = startDateOfWeek.Date;
            DateTime endDateOfWeek = startDate.AddDays(7);
            endDateOfWeek = endDateOfWeek.Date;

            var minutes = DataContext.Get<Shift>().Where(s => s.EmployeeId == employeeId 
                && s.StartTime >= startDateOfWeek && s.EndTime <= endDateOfWeek).Sum(s => (DbFunctions.DiffMinutes(s.StartTime, s.EndTime)));
            if (minutes == null)
                return 0;
            var result = ((float)minutes) / 60.0f;
            return result;
        }
        public bool HasFourConsetiveSundays(DateTime shiftDate)
        {
            if (shiftDate.DayOfWeek != DayOfWeek.Sunday)
                return false;
            var oneWeekBefore = shiftDate.AddDays(-7).Date;
            var twoWeeksBefore = shiftDate.AddDays(-14).Date;
            var threeWeeksBefore = shiftDate.AddDays(-21).Date;
            var count = 0;

            if (DataContext.Get<Shift>().Any(s => s.StartTime.Date == oneWeekBefore))
                count++;
            if (count == 0)
                return false;

            if (DataContext.Get<Shift>().Any(s => s.StartTime.Date == twoWeeksBefore))
                count++;
            if (count == 1)
                return false;

            if (DataContext.Get<Shift>().Any(s => s.StartTime.Date == threeWeeksBefore))
                count++;

            return count >= 3;
        }
    }
}
