using System.Collections.Generic;
using System.Web.Mvc;
using Eagle.Core.Settings;
using Eagle.Entities.Services.Messaging;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.Services.Messaging
{
    public interface INotificationTargetRepository : IRepositoryBase<NotificationTarget>
    {
        IEnumerable<NotificationTarget> GetNotificationTargets(int notificationTypeId);
        NotificationTarget GetDetails(int notificationTypeId, NotificationTargetType notificationTargetTypeId);
        MultiSelectList PopulateAvailableNotificationTargetTypes(int? notificationTypeId = null);
        MultiSelectList PopulateSelectedNotificationTargetTypes(int? notificationTypeId = null);
    }
}
