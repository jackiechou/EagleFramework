using System;
using System.Collections.Generic;
using Eagle.Entities.SystemManagement;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.SystemManagement
{
    public interface ILogRepository : IRepositoryBase<Log>
    {
        IEnumerable<Log> GetListByApplicationId(Guid applicationId, ref int? recordCount, string orderBy = null,
            int? page = null, int? pageSize = null);
    }
}
