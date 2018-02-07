using System;
using System.Collections.Generic;
using Eagle.Entities.SystemManagement;
using Eagle.Repositories;
using Eagle.Services.Dtos.SystemManagement;
using Eagle.Services.EntityMapping.Common;
using Eagle.Services.Exceptions;

namespace Eagle.Services.SystemManagement
{   
    /// <summary>
    /// Trace writer to direct tracing to Log4Net in the Web API way.
    /// See these articles for more information about tracing in Web API:
    /// http://www.asp.net/web-api/overview/testing-and-debugging/tracing-in-aspnet-web-api
    /// http://blogs.msdn.com/b/roncain/archive/2012/04/12/tracing-in-asp-net-web-api.aspx
    /// </summary>
    public class LogService : BaseService, ILogService
    {
        public LogService(IUnitOfWork unitOfWork) : base(unitOfWork) 
        {
        }

        #region Log to database

        public IEnumerable<LogDetail> GetListByApplicationId(Guid applicationId, ref int? recordCount, string orderBy = null, int? page = null, int? pageSize = null)
        {
            var lst = UnitOfWork.LogRepository.GetListByApplicationId(applicationId, ref recordCount, orderBy, page,
                pageSize);
            return lst.ToDtos<Log, LogDetail>();
        }
        public void ClearLog(Guid applicationId)
        {
            int? recordCount = null;
            var lst = UnitOfWork.LogRepository.GetListByApplicationId(applicationId, ref recordCount);
            if (lst == null) return;
            foreach (var item in lst)
            {
                UnitOfWork.LogRepository.Delete(item);
            }
        }
        public void InsertLog(Guid userId, LogEntry entry)
        {
            var entity = entry.ToEntity<LogEntry, Log>();
            entity.Logger = userId;
            entity.LogDate = DateTime.UtcNow;
            UnitOfWork.LogRepository.Insert(entity);
        }
        public void DeleteLog(int logId)
        {
            var entity = UnitOfWork.LogRepository.FindById(logId);
            if (entity == null) throw new NotFoundExceptionLog();
            UnitOfWork.LogRepository.Delete(entity);
        }

        #endregion




    }
}
