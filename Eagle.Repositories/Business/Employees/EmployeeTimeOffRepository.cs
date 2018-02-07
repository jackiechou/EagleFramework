using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Eagle.Core.Settings;
using Eagle.Entities.Business.Employees;
using Eagle.EntityFramework;

namespace Eagle.Repositories.Business.Employees
{
    public class EmployeeTimeOffRepository : RepositoryBase<EmployeeTimeOff>, IEmployeeTimeOffRepository
    {
        public EmployeeTimeOffRepository(IDataContext dataContext) : base(dataContext) { }
        public int GetByTimeRangeForCurrentMember(int memberId, DateTime startDate,
           DateTime endDate)
        {
            return DataContext.Get<EmployeeTimeOff>().Count(s => s.EmployeeId == memberId && DbFunctions.TruncateTime(s.StartDate) >= startDate.Date && DbFunctions.TruncateTime(s.EndDate) <= endDate.Date);
        }

        public int GetMemberTimeOffsForGroup(int memberId, int rosterId, DateTime startDate, DateTime endDate, int? status)
        {
            return DataContext.Get<EmployeeTimeOff>().Count(s => s.EmployeeId == memberId && s.StartDate >= startDate && s.EndDate <= endDate);
        }

        public bool IsMemberTimeOffDateTimeOverlap(int employeeId, DateTime startDate, DateTime endDate, int? employeeTimeOffId = null)
        {
            return DataContext.Get<EmployeeTimeOff>().Any(s => s.EmployeeId == employeeId && s.EmployeeTimeOffId != employeeTimeOffId
                && s.StartDate < endDate && s.EndDate > startDate
                && s.Status != TimeOffStatus.Cancel && s.Status != TimeOffStatus.Rejected);
        }
        public IEnumerable<EmployeeTimeOff> GetMemberTimeOffsByTimeOffType(int timeOffTypeId)
        {
            return DataContext.Get<EmployeeTimeOff>().Where(s => s.TimeOffTypeId == timeOffTypeId);
        }
    }
}
