using System;
using System.Collections.Generic;
using Eagle.Entities.Business.Employees;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.Business.Employees
{
    public interface IEmployeeTimeOffRepository : IRepositoryBase<EmployeeTimeOff>
    {
        int GetByTimeRangeForCurrentMember(int memberId, DateTime startDate,
            DateTime endDate);

        int GetMemberTimeOffsForGroup(int memberId, int rosterId, DateTime startDate, DateTime endDate, int? status);

        bool IsMemberTimeOffDateTimeOverlap(int employeeId, DateTime startDate, DateTime endDate,
            int? employeeTimeOffId = null);

        IEnumerable<EmployeeTimeOff> GetMemberTimeOffsByTimeOffType(int timeOffTypeId);
    }
}
