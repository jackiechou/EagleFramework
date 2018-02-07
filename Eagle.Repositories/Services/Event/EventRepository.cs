using System;
using System.Collections.Generic;
using System.Linq;
using Eagle.Common.Utilities;
using Eagle.Core.Common;
using Eagle.Core.Settings;
using Eagle.Entities.Services.Events;
using Eagle.EntityFramework;

namespace Eagle.Repositories.Services.Event
{
    public class EventRepository: RepositoryBase<Entities.Services.Events.Event>, IEventRepository
    {
        public EventRepository(IDataContext dataContext): base(dataContext){ }

        public IEnumerable<EventInfo> Search(out int recordCount, string searchText = null, int? typeId = null, DateTime? fromDate = null, DateTime? toDate = null, EventStatus? status = null, string orderBy = null, int? page = null, int? pageSize = null)
        {
            var queryable = from x in DataContext.Get<Entities.Services.Events.Event>()
                            join y in DataContext.Get<EventType>() on x.TypeId equals y.TypeId into catelist
                            from cate in catelist.DefaultIfEmpty()
                            where (status == null || x.Status == status)
                            select new EventInfo
                            {
                                TypeId = x.TypeId,
                                EventId = x.EventId,
                                EventCode = x.EventCode,
                                EventTitle = x.EventTitle,
                                EventMessage = x.EventMessage,
                                StartDate = x.StartDate,
                                EndDate = x.EndDate,
                                TimeZone = x.TimeZone,
                                Location = x.Location,
                                Latitude = x.Latitude,
                                Longitude = x.Longitude,
                                SmallPhoto = x.SmallPhoto,
                                LargePhoto = x.LargePhoto,
                                IsNotificationUsed = x.IsNotificationUsed,
                                Status = x.Status,
                                CreatedDate = x.CreatedDate,
                                LastModifiedDate = x.LastModifiedDate,
                                CreatedByUserId = x.CreatedByUserId,
                                LastModifiedByUserId = x.LastModifiedByUserId,
                                Ip = x.Ip,
                                LastUpdatedIp = x.LastUpdatedIp
                            };
            if (typeId != null && typeId > 0)
            {
                queryable = queryable.Where(x => x.TypeId == typeId);
            }
            if (fromDate != null)
            {
                queryable = queryable.Where(x => x.StartDate >= fromDate);
            }
            if (toDate != null)
            {
                queryable = queryable.Where(x => x.EndDate <= toDate);
            }
            if (!string.IsNullOrEmpty(searchText))
            {
                queryable = queryable.Where(x => x.EventTitle.ToLower().Contains(searchText.ToLower()));
            }
           
            return queryable.AsEnumerable().WithRecordCount(out recordCount)
                            .WithSortingAndPaging(orderBy, page, pageSize);
        }

        public IEnumerable<EventInfo> GetEvents(out int recordCount, int? typeId = null, EventStatus? status = null, string orderBy = null, int? page = null, int? pageSize = null)
        {
            var queryable = from x in DataContext.Get<Entities.Services.Events.Event>()
                            join y in DataContext.Get<EventType>() on x.TypeId equals y.TypeId into catelist
                            from cate in catelist.DefaultIfEmpty()
                            where (status == null || x.Status == status)
                            select new EventInfo
                            {
                                TypeId = x.TypeId,
                                EventId = x.EventId,
                                EventCode = x.EventCode,
                                EventTitle = x.EventTitle,
                                EventMessage = x.EventMessage,
                                StartDate = x.StartDate,
                                EndDate = x.EndDate,
                                TimeZone = x.TimeZone,
                                Location = x.Location,
                                Latitude = x.Latitude,
                                Longitude = x.Longitude,
                                SmallPhoto = x.SmallPhoto,
                                LargePhoto = x.LargePhoto,
                                IsNotificationUsed = x.IsNotificationUsed,
                                Status = x.Status,
                                CreatedDate = x.CreatedDate,
                                LastModifiedDate = x.LastModifiedDate,
                                CreatedByUserId = x.CreatedByUserId,
                                LastModifiedByUserId = x.LastModifiedByUserId,
                                Ip = x.Ip,
                                LastUpdatedIp = x.LastUpdatedIp
                            };
            if (typeId != null && typeId > 0)
            {
                queryable = queryable.Where(x => x.TypeId == typeId);
            }

            return queryable.AsEnumerable().WithRecordCount(out recordCount)
                            .WithSortingAndPaging(orderBy, page, pageSize);
        }

        public EventInfo GetDetails(int id)
        {
            return (from e in DataContext.Get<Entities.Services.Events.Event>()
                    join t in DataContext.Get<EventType>() on e.TypeId equals t.TypeId into etJoin
                    from type in etJoin.DefaultIfEmpty()
                    where e.EventId == id
                    select new EventInfo
                    {
                        TypeId = e.TypeId,
                        EventId = e.EventId,
                        EventCode = e.EventCode,
                        EventTitle = e.EventTitle,
                        EventMessage = e.EventMessage,
                        StartDate = e.StartDate,
                        EndDate = e.EndDate,
                        TimeZone = e.TimeZone,
                        Location = e.Location,
                        Latitude = e.Latitude,
                        Longitude = e.Longitude,
                        SmallPhoto = e.SmallPhoto,
                        LargePhoto = e.LargePhoto,
                        IsNotificationUsed = e.IsNotificationUsed,
                        Status = e.Status,
                        CreatedDate = e.CreatedDate,
                        LastModifiedDate = e.LastModifiedDate,
                        CreatedByUserId = e.CreatedByUserId,
                        LastModifiedByUserId = e.LastModifiedByUserId,
                        Ip = e.Ip,
                        LastUpdatedIp = e.LastUpdatedIp,
                        EventType = type
                    }).FirstOrDefault();
        }
        public bool HasDataExisted(int typeId, string title)
        {
            var query = DataContext.Get<Entities.Services.Events.Event>().FirstOrDefault(c => c.TypeId == typeId && c.EventTitle.ToUpper().Equals(title.ToUpper()));
            return (query != null);
        }

        public bool HasCodeExisted(string code)
        {
            var query = DataContext.Get<Entities.Services.Events.Event>().FirstOrDefault(c => c.EventCode.ToUpper().Equals(code.ToUpper()));
            return (query != null);
        }

        public string GenerateCode(int maxLetters)
        {
            int newId = 1;
            var query = from u in DataContext.Get<Entities.Services.Events.Event>() select u.EventId;
            if (query.Any())
            {
                newId = query.Max() + 1;
            }
            return StringUtils.GenerateCode(newId.ToString(), maxLetters);
        }
    }
}
