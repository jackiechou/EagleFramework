using Eagle.Entities.Business.Employees;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.Business.Employees
{
    public interface ITerminationRepository: IRepositoryBase<Termination>
    {
        Termination GetTerminationDetailByEmployeeId(int empId);
    }
}
