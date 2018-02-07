using Eagle.Entities.Business.Employees;
using Eagle.EntityFramework;

namespace Eagle.Repositories.Business.Employees
{
    public class SalaryRepository : RepositoryBase<Salary>, ISalaryRepository
    {
        public SalaryRepository(IDataContext dataContext) : base(dataContext) { }

    }
}
