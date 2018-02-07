using System.Collections.Generic;
using System.Linq;
using Eagle.Core.Common;
using Eagle.Entities.SystemManagement;
using Eagle.EntityFramework;

namespace Eagle.Repositories.SystemManagement
{
    public class ApplicationRepository: RepositoryBase<ApplicationEntity>, IApplicationRepository
    {
        public ApplicationRepository(IDataContext dataContext) : base(dataContext) { }
        public IEnumerable<ApplicationEntity> GetList(ref int? recordCount, string orderBy = null, int? page = null, int? pageSize = null)
        {
            return DataContext.Get<ApplicationEntity>().WithRecordCount(ref recordCount)
                            .WithSortingAndPaging(orderBy, page, pageSize);
        }
        public bool HasDataExisted(string applicationName)
        {
            var result = DataContext.Get<ApplicationEntity>().FirstOrDefault(x => x.ApplicationName == applicationName);
            return (result != null);
        }
    }
}
