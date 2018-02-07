using Eagle.Entities.Business.Employees;
using Eagle.EntityFramework;

namespace Eagle.Repositories.Business.Employees
{
    public class RewardDisciplineRepository: RepositoryBase<RewardDiscipline>, IRewardDisciplineRepository
    {
        public RewardDisciplineRepository(IDataContext dataContext) : base(dataContext) { }

    }
}
