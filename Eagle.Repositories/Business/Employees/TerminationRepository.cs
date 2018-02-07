using System.Linq;
using Eagle.Entities.Business.Employees;
using Eagle.EntityFramework;

namespace Eagle.Repositories.Business.Employees
{
    public class TerminationRepository: RepositoryBase<Termination>, ITerminationRepository
    {
        public TerminationRepository(IDataContext dataContext) : base(dataContext) { }
        public Termination GetTerminationDetailByEmployeeId(int empId)
        {
            return (from h in DataContext.Get<Termination>()
                    join e in DataContext.Get<Employee>() on h.EmployeeId equals e.EmployeeId into heJoin
                    from employee in heJoin.DefaultIfEmpty()
                    where h.EmployeeId == empId
                    select h).FirstOrDefault();
        }
    }
}
