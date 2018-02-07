using System.Collections.Generic;
using Eagle.Entities.Business.Employees;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.Business
{
    public interface IWorkingHistoryRepository: IRepositoryBase<WorkingHistory>
    {
        IEnumerable<WorkingHistory> GetWorkingHistories(int empId);
    }
}
