using System;
using System.Collections.Generic;
using Eagle.Services.Dtos.SystemManagement;

namespace Eagle.Services.SystemManagement
{
    public interface ILogService : IBaseService
    {
        #region Log to database
        IEnumerable<LogDetail> GetListByApplicationId(Guid applicationId, ref int? recordCount, string orderBy = null,
            int? page = null, int? pageSize = null);
        void ClearLog(Guid applicationId);
        void InsertLog(Guid userId, LogEntry entry);
        void DeleteLog(int logId);

        #endregion
    }
}
