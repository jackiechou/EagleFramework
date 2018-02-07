using System.Collections.Generic;
using Eagle.Entities.Business.Employees;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.Business.Employees
{
    public interface ISkillRepository : IRepositoryBase<Skill>
    {
        IEnumerable<Skill> GetSkills(string filter, int? page, int? pageSize, ref int? recordCount);
        IEnumerable<Skill> GetSkillsByEmployee(int employeeId);
        List<int> GetSkillsByEmployeeId(int employeeId);
        bool HasNameExisted(string name);

    }
}
