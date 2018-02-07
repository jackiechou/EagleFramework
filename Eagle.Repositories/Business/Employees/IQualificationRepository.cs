using Eagle.Entities.Business.Employees;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.Business.Employees
{
    public interface IQualificationRepository : IRepositoryBase<Qualification>
    {
        string GenerateQualificationNo(int maxLetters);
        bool HasQualificationNoExisted(string qualificationNo);
    }
}
