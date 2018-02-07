using System.Linq;
using Eagle.Entities.Business.Employees;
using Eagle.EntityFramework;
using Eagle.Repositories.Business.Employees;

namespace Eagle.Repositories.Business.Roster
{
    public class JobPositionSkillRepository: RepositoryBase<JobPositionSkill>, IJobPositionSkillRepository
    {
        public JobPositionSkillRepository(IDataContext dataContext) : base(dataContext) { }
        public JobPositionSkill GetDetails(int positionId, int skillId)
        {
            return DataContext.Get<JobPositionSkill>().FirstOrDefault(s => s.PositionId == positionId && s.SkillId == skillId);
        }
    }
}
