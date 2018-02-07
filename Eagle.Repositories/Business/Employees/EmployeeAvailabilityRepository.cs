using System;
using System.Collections.Generic;
using System.Linq;
using Eagle.Entities.Business.Employees;
using Eagle.EntityFramework;

namespace Eagle.Repositories.Business.Employees
{
    public class EmployeeAvailabilityRepository : RepositoryBase<EmployeeAvailability>, IEmployeeAvailabilityRepository
    {
        public EmployeeAvailabilityRepository(IDataContext dataContext) : base(dataContext) { }
        public IEnumerable<EmployeeAvailability> GetEmployeeAvailabilitiesByTimeRange(DateTime startDate, DateTime endDate)
        {
            var rosterMemberAvailabilityQueryable = DataContext.Get<EmployeeAvailability>().AsQueryable().Where(s => s.StartDate >= startDate && s.EndDate <= endDate);
            return rosterMemberAvailabilityQueryable.AsEnumerable();
        }

        public IEnumerable<EmployeeAvailability> GetUnAvailabilitiesByTimeRange(DateTime startDate, DateTime endDate)
        {
            var listAvai = DataContext.Get<EmployeeAvailability>().AsQueryable().Where(s => s.StartDate >= startDate && s.EndDate <= endDate).OrderBy(s => s.StartDate).ToList();
            var listUnAvai = new List<EmployeeAvailability>();
            if (listAvai.Any())
            {
                var firstItem = listAvai[0];
                var tmp = firstItem;
                for (int i = 1; i < listAvai.Count; i++)
                {
                    var nextItem = listAvai[i];
                    var newItem = new EmployeeAvailability()
                    {
                        StartDate = tmp.EndDate,
                        EndDate = nextItem.StartDate
                    };
                    listUnAvai.Add(newItem);
                    tmp = nextItem;
                }
                var leftGap = new EmployeeAvailability()
                {
                    StartDate = firstItem.StartDate.Date,
                    EndDate = firstItem.StartDate
                };
                var rightGap = new EmployeeAvailability()
                {
                    StartDate = tmp.EndDate,
                    EndDate = tmp.EndDate.AddDays(1).Date
                };

                listUnAvai.Add(leftGap);
                listUnAvai.Add(rightGap);
            }
            else
            {
                return listAvai;
            }
            return listUnAvai;
        }

        public IEnumerable<EmployeeAvailability> GetAvailabilitiesByTimeRangeForCurrentEmployee(int employeeId, DateTime startDate, DateTime endDate)
        {
            var rosterMemberAvailabilityQueryable = DataContext.Get<EmployeeAvailability>().AsQueryable().Where(s => s.EmployeeId == employeeId && s.StartDate >= startDate && s.EndDate <= endDate);
            return rosterMemberAvailabilityQueryable.AsEnumerable();
        }

        public int? GetTotalAvailabilitiesByTimeRangeForCurrentEmployee(int employeeId, DateTime startDate, DateTime endDate)
        {
            return DataContext.Get<EmployeeAvailability>().Count(s => s.EmployeeId == employeeId && s.StartDate >= startDate && s.EndDate <= endDate);
        }

        public int? GetOverlapAvailabilityByEmployeeId(int employeeId, DateTime startDate, DateTime endDate, int? id = null)
        {
            return DataContext.Get<EmployeeAvailability>()
                .Count(s => s.EmployeeId == employeeId && s.EmployeeId != id && (
                    (s.StartDate <= startDate && startDate <= s.EndDate)
                    || (s.StartDate < endDate && endDate < s.EndDate)
                   || (startDate < s.StartDate && s.EndDate < endDate)
                ));
        }
    }
}
