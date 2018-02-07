using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Eagle.Common.Extensions.EnumHelper;
using Eagle.Core.Settings;
using Eagle.Entities.Services.Messaging;
using Eagle.EntityFramework;

namespace Eagle.Repositories.Services.Messaging
{
    public class NotificationTargetRepository : RepositoryBase<NotificationTarget>, INotificationTargetRepository
    {
        public NotificationTargetRepository(IDataContext dataContext) : base(dataContext)
        {
        }
        //public NotificationTargetInfo GetDefaultNotificationSender()
        //{
        //    return (from t in DataContext.Get<NotificationSender>()
        //            where t.IsSelected == true
        //            select new NotificationTargetInfo
        //            {

        //            }).FirstOrDefault();
        //}
        public IEnumerable<NotificationTarget> GetNotificationTargets(int notificationTypeId)
        {
            return (from x in DataContext.Get<NotificationTarget>()
                where x.NotificationTypeId == notificationTypeId
                select x).AsEnumerable();
        }

        public NotificationTarget GetDetails(int notificationTypeId, NotificationTargetType notificationTargetTypeId)
        {
            return (from x in DataContext.Get<NotificationTarget>()
                    where x.NotificationTypeId == notificationTypeId && x.NotificationTargetTypeId == notificationTargetTypeId
                    select x).FirstOrDefault();
        }

        public MultiSelectList PopulateAvailableNotificationTargetTypes(int? notificationTypeId = null)
        {
            var enumValues = Enum.GetValues(typeof(NotificationTargetType)).Cast<NotificationTargetType>();

            var types = (from enumValue in enumValues
                        select new SelectListItem
                        {
                                Text = enumValue.DisplayName(),
                                Value = enumValue.ToString()
                            }).ToList();

            if (notificationTypeId == null || notificationTypeId <= 0)
            {
                return new MultiSelectList(types, "Value", "Text");
            }

            var selectedTargetTypes = (from t in DataContext.Get<NotificationTarget>()
                                       where t.NotificationTypeId == notificationTypeId
                                       select t.NotificationTargetTypeId.ToString()).ToArray();
            if (selectedTargetTypes.Any())
            {
                types = types.Where(type => !selectedTargetTypes.Contains(type.Value)).ToList();
            }
            return new MultiSelectList(types, "Value", "Text", selectedTargetTypes);
        }


        public MultiSelectList PopulateSelectedNotificationTargetTypes(int? notificationTypeId = null)
        {
            if (notificationTypeId == null || notificationTypeId <= 0) return null;

            var selectedTargetTypes = (from t in DataContext.Get<NotificationTarget>()
                where t.NotificationTypeId == notificationTypeId
                select t.NotificationTargetTypeId.ToString()).ToArray();
            if (!selectedTargetTypes.Any()) return null;

            var enumValues = Enum.GetValues(typeof(NotificationTargetType)).Cast<NotificationTargetType>();

            var types = (from enumValue in enumValues
                         select new SelectListItem
                         {
                             Text = enumValue.DisplayName(),
                             Value = enumValue.ToString()
                         }).ToList();

            types = types.Where(type => selectedTargetTypes.Contains(type.Value)).ToList();
            return new MultiSelectList(types, "Value", "Text", selectedTargetTypes);
        }
    }
}
