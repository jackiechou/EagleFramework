using System;
using System.Collections.Generic;
using Eagle.Entities.Business.Employees;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.Business.Employees
{
    public interface IEmployeeAvailabilityRepository : IRepositoryBase<EmployeeAvailability>
    {
        IEnumerable<EmployeeAvailability> GetEmployeeAvailabilitiesByTimeRange(DateTime startDate, DateTime endDate);
        IEnumerable<EmployeeAvailability> GetUnAvailabilitiesByTimeRange(DateTime startDate, DateTime endDate);

        IEnumerable<EmployeeAvailability> GetAvailabilitiesByTimeRangeForCurrentEmployee(int employeeId,
            DateTime startDate, DateTime endDate);

        int? GetTotalAvailabilitiesByTimeRangeForCurrentEmployee(int employeeId, DateTime startDate, DateTime endDate);
        int? GetOverlapAvailabilityByEmployeeId(int employeeId, DateTime startDate, DateTime endDate, int? id = null);
    }
}
