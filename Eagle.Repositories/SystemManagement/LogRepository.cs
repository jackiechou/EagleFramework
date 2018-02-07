using System;
using System.Collections.Generic;
using System.Linq;
using Eagle.Core.Common;
using Eagle.Entities.SystemManagement;
using Eagle.EntityFramework;

namespace Eagle.Repositories.SystemManagement
{
    public class LogRepository : RepositoryBase<Log>, ILogRepository
    {
        public LogRepository(IDataContext dataContext) : base(dataContext) { }

        public IEnumerable<Log> GetListByApplicationId(Guid applicationId, ref int? recordCount, string orderBy = null, int? page = null, int? pageSize = null)
        {
            return DataContext.Get<Log>().Where(x => x.ApplicationId == applicationId).WithRecordCount(ref recordCount)
                .WithSortingAndPaging(orderBy, page, pageSize);
        }
        
    }
}
