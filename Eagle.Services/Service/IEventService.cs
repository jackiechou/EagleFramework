using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Eagle.Core.Settings;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Dtos.Services.Event;

namespace Eagle.Services.Service
{
    public interface IEventService: IBaseService
    {
        #region Event Type
        IEnumerable<TreeDetail> GetEventTypeSelectTree(EventTypeStatus? status, int? selectedId,
            bool? isRootShowed = false);

        EventTypeDetail GetEventTypeDetail(int id);
        void InsertEventType(Guid userId, EventTypeEntry entry);
        void UpdateEventType(Guid userId, EventTypeEditEntry entry);
        void UpdateEventTypeStatus(Guid userId, int id, EventTypeStatus status);
        void UpdateEventTypeListOrder(Guid userId, int id, int listOrder);
        #endregion

        #region Event

        IEnumerable<EventInfoDetail> Search(EventSearchEntry searchEntry, out int recordCount, string orderBy = null,
            int? page = 1, int? pageSize = null);

        IEnumerable<EventInfoDetail> GetEvents(int? typeId, EventStatus? status, out int recordCount,
            string orderBy = null, int? page = 1, int? pageSize = null);
        string GenerateCode(int maxLetters);
        SelectList PoplulateTimeZoneSelectList(string selectedValue = null, bool? isShowSelectText = true);
        EventInfoDetail GeEventDetail(int id);
        EventDetail InsertEvent(Guid applicationId, Guid userId, int vendorId, EventEntry entry);
        void UpdateEvent(Guid applicationId, Guid userId, int vendorId, EventEditEntry entry);
        void UpdateEventStatus(Guid applicationId, Guid userId, int vendorId, int id, EventStatus status);

        #endregion
    }
}
