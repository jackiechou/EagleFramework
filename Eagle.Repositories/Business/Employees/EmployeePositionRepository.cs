using System.Linq;
using Eagle.Entities.Business.Employees;
using Eagle.EntityFramework;

namespace Eagle.Repositories.Business.Employees
{
    public class EmployeePositionRepository : RepositoryBase<EmployeePosition>, IEmployeePositionRepository
    {
        public EmployeePositionRepository(IDataContext dataContext) : base(dataContext) { }
        public EmployeePosition GetEmployeePositionDetail(int employeeId, int positionId)
        {
            return DataContext.Get<EmployeePosition>().FirstOrDefault(m => m.EmployeeId == employeeId && m.PositionId == positionId);
        }
    }
}
