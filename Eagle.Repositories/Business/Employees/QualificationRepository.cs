using System.Linq;
using Eagle.Common.Utilities;
using Eagle.Entities.Business.Employees;
using Eagle.EntityFramework;

namespace Eagle.Repositories.Business.Employees
{
    public class QualificationRepository : RepositoryBase<Qualification>, IQualificationRepository
    {
        public QualificationRepository(IDataContext dataContext) : base(dataContext) { }

        public string GenerateQualificationNo(int maxLetters)
        {
            int newId = 1;
            var query = from u in DataContext.Get<Qualification>() select u.QualificationId;
            if (query.Any())
            {
                newId = query.Max() + 1;
            }
            return StringUtils.GenerateCode(newId.ToString(), maxLetters);
        }


        public bool HasQualificationNoExisted(string qualificationNo)
        {
            var query = from p in DataContext.Get<Qualification>()
                        where p.QualificationNo.ToLower().Trim() == qualificationNo.ToLower().Trim() select p;
            return query.Any();
        }
    }
}
