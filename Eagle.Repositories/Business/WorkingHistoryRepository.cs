using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Eagle.Entities.Business.Employees;
using Eagle.EntityFramework;

namespace Eagle.Repositories.Business
{
    public class WorkingHistoryRepository: RepositoryBase<WorkingHistory>, IWorkingHistoryRepository
    {
        public WorkingHistoryRepository(IDataContext dataContext) : base(dataContext) { }

        public IEnumerable<WorkingHistory> GetWorkingHistories(int empId)
        {
            return (from h in DataContext.Get<WorkingHistory>().Include(x => x.Employee).Include(x => x.Position)
                       join e in DataContext.Get<Employee>() on h.EmployeeId equals e.EmployeeId into heJoin
                       from employee in heJoin.DefaultIfEmpty()
                       join p in DataContext.Get<JobPosition>() on h.PositionId equals p.PositionId into hpJoin
                       from position in hpJoin.DefaultIfEmpty()
                       where h.EmployeeId == empId
                       select h).OrderByDescending(h => h.WorkingHistoryId).AsEnumerable();
        }
    }
}
