using System.Collections.Generic;
using Eagle.Entities.Services.Messaging;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.Services.Messaging
{
    public interface INotificationTargetDefaultRepository : IRepositoryBase<NotificationTargetDefault>
    {
        IEnumerable<NotificationTargetDefault> GetNotificationTargetDefaults();
        IEnumerable<NotificationTargetDefaultInfo> GetNotificationTargetDefaults(int? mailServerProviderId);
        NotificationTargetDefault GetNotificationTargetDefault();
        NotificationTargetDefault FindByEmailAddress(string mailAddress);
        NotificationTargetDefault GetDetails(string mailAddress);
        bool HasTargetNameExisted(string targetName);
        bool HasMailAddressExisted(string mailAddress);
    }
}
