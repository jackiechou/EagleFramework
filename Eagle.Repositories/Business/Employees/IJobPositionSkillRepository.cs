using Eagle.Entities.Business.Employees;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.Business.Employees
{
    public interface IJobPositionSkillRepository: IRepositoryBase<JobPositionSkill>
    {
        JobPositionSkill GetDetails(int positionId, int skillId);
    }
}
