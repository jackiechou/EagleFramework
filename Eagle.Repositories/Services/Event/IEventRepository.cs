using System;
using System.Collections.Generic;
using Eagle.Core.Settings;
using Eagle.Entities.Services.Events;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.Services.Event
{
    public interface IEventRepository: IRepositoryBase<Entities.Services.Events.Event>
    {
        IEnumerable<EventInfo> Search(out int recordCount, string searchText = null, int? typeId = null,
            DateTime? fromDate = null, DateTime? toDate = null, EventStatus? status = null, string orderBy = null,
            int? page = null, int? pageSize = null);

        IEnumerable<EventInfo> GetEvents(out int recordCount, int? typeId = null, EventStatus? status = null,
            string orderBy = null, int? page = null, int? pageSize = null);
        EventInfo GetDetails(int id);
        bool HasDataExisted(int typeId, string title);
        bool HasCodeExisted(string code);
        string GenerateCode(int maxLetters);
    }
}
