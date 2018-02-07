using System.Collections.Generic;
using Eagle.Entities.SystemManagement;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.SystemManagement
{
    public interface IApplicationRepository : IRepositoryBase<ApplicationEntity>
    {
        IEnumerable<ApplicationEntity> GetList(ref int? recordCount, string orderBy = null,
            int? page = null, int? pageSize = null);
        bool HasDataExisted(string applicationName);
    }
}
