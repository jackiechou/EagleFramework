using Eagle.Entities.Business.Employees;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.Business.Employees
{
    public interface IEmployeeSkillRepository : IRepositoryBase<EmployeeSkill>
    {
        EmployeeSkill GetEmployeeSkillDetail(int employeeId, int skillId);
    }
}
