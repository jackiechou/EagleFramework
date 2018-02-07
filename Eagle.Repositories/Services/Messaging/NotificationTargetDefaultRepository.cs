using System.Collections.Generic;
using System.Linq;
using Eagle.Entities.Services.Messaging;
using Eagle.EntityFramework;

namespace Eagle.Repositories.Services.Messaging
{
    public class NotificationTargetDefaultRepository : RepositoryBase<NotificationTargetDefault>, INotificationTargetDefaultRepository
    {
        public NotificationTargetDefaultRepository(IDataContext dataContext) : base(dataContext) { }
        public IEnumerable<NotificationTargetDefault> GetNotificationTargetDefaults()
        {
            return (from a in DataContext.Get<NotificationTargetDefault>()
                    select a).AsEnumerable();
        }

        public IEnumerable<NotificationTargetDefaultInfo> GetNotificationTargetDefaults(int? mailServerProviderId)
        {
            var query = from a in DataContext.Get<NotificationTargetDefault>()
                        join p in DataContext.Get<MailServerProvider>() on a.MailServerProviderId equals p.MailServerProviderId into apJoin
                        from provider in apJoin.DefaultIfEmpty()
                        where mailServerProviderId == null || a.MailServerProviderId == mailServerProviderId
                        orderby a.NotificationTargetDefaultId
                        select new NotificationTargetDefaultInfo
                        {
                            NotificationTargetDefaultId = a.NotificationTargetDefaultId,
                            MailServerProviderId = a.MailServerProviderId,
                            TargetName = a.TargetName,
                            ContactName = a.ContactName,
                            Address = a.Address,
                            MailAddress = a.MailAddress,
                            Password = a.Password,
                            IsSelected = a.IsSelected,
                            IsActive = a.IsActive,
                            MailServerProvider = provider
                        };

            return query.AsEnumerable();
        }

        public NotificationTargetDefault GetNotificationTargetDefault()
        {
            return DataContext.Get<NotificationTargetDefault>().FirstOrDefault(x => x.IsSelected == true);
        }

        public NotificationTargetDefault FindByEmailAddress(string mailAddress)
        {
            return DataContext.Get<NotificationTargetDefault>().FirstOrDefault(x => x.MailAddress == mailAddress);
        }

        public NotificationTargetDefault GetDetails(string mailAddress)
        {
            return (from x in DataContext.Get<NotificationTargetDefault>()
                    where x.MailAddress == mailAddress
                    select x).FirstOrDefault();
        }
        
        public bool HasTargetNameExisted(string targetName)
        {
            var query = DataContext.Get<NotificationTargetDefault>().FirstOrDefault(p => p.TargetName.Equals(targetName));
            return (query != null);
        }

        public bool HasMailAddressExisted(string mailAddress)
        {
            var query = DataContext.Get<NotificationTargetDefault>().FirstOrDefault(p => p.MailAddress.ToLower().Equals(mailAddress.ToLower()));
            return (query != null);
        }

    }
}
