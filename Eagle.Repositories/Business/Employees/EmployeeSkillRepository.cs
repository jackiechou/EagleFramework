using System.Linq;
using Eagle.Entities.Business.Employees;
using Eagle.EntityFramework;

namespace Eagle.Repositories.Business.Employees
{
    public class EmployeeSkillRepository : RepositoryBase<EmployeeSkill>, IEmployeeSkillRepository
    {
        public EmployeeSkillRepository(IDataContext dataContext) : base(dataContext) { }
        public EmployeeSkill GetEmployeeSkillDetail(int employeeId, int skillId)
        {
            return DataContext.Get<EmployeeSkill>().FirstOrDefault(m => m.EmployeeId == employeeId && m.SkillId == skillId);
        }
    }
}
