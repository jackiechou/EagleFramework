using Eagle.Entities.Business.Employees;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.Business.Employees
{
    public interface IEmployeePositionRepository : IRepositoryBase<EmployeePosition>
    {
        EmployeePosition GetEmployeePositionDetail(int employeeId, int positionId);
    }
}
