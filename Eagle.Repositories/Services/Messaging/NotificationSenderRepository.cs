using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Eagle.Entities.Services.Messaging;
using Eagle.EntityFramework;
using Eagle.Resources;

namespace Eagle.Repositories.Services.Messaging
{
    public class NotificationSenderRepository : RepositoryBase<NotificationSender>, INotificationSenderRepository
    {
        public NotificationSenderRepository(IDataContext dataContext) : base(dataContext) {}
        public IEnumerable<NotificationSender> GetNotificationSenders()
        {
           return (from a in DataContext.Get<NotificationSender>()
                         select a).AsEnumerable();
        }
        public IEnumerable<NotificationSenderInfo> GetNotificationSenders(int? mailServerProviderId)
        {
            var query = from a in DataContext.Get<NotificationSender>()
                        join p in DataContext.Get<MailServerProvider>() on a.MailServerProviderId equals p.MailServerProviderId into apJoin
                        from provider in apJoin.DefaultIfEmpty()
                        where mailServerProviderId ==null || a.MailServerProviderId == mailServerProviderId
                        orderby a.NotificationSenderId
                        select new NotificationSenderInfo
                        {
                            NotificationSenderId = a.NotificationSenderId,
                            MailServerProviderId = a.MailServerProviderId,
                            SenderName = a.SenderName,
                            ContactName = a.ContactName,
                            MailAddress = a.MailAddress,
                            Password = a.Password,
                            Signature = a.Signature,
                            IsSelected = a.IsSelected,
                            IsActive = a.IsActive,
                            MailServerProvider = provider
                        };
            
            return query.AsEnumerable();
        }
        public NotificationSenderInfo GetDefaultNotificationSender()
        {
            return (from a in DataContext.Get<NotificationSender>()
                    join p in DataContext.Get<MailServerProvider>() on a.MailServerProviderId equals p.MailServerProviderId into apJoin
                    from provider in apJoin.DefaultIfEmpty()
                    orderby a.NotificationSenderId
                    where a.IsSelected == true
                    select new NotificationSenderInfo
                    {
                        NotificationSenderId = a.NotificationSenderId,
                        MailServerProviderId = a.MailServerProviderId,
                        SenderName = a.SenderName,
                        ContactName = a.ContactName,
                        MailAddress = a.MailAddress,
                        Password = a.Password,
                        Signature = a.Signature,
                        IsSelected = a.IsSelected,
                        IsActive = a.IsActive,
                        MailServerProvider = provider
                    }).FirstOrDefault();
        }
        public NotificationSender FindByEmailAddress(string mailAddress)
        {
            return DataContext.Get<NotificationSender>().FirstOrDefault(x => x.MailAddress == mailAddress);
        }
        public NotificationSenderInfo GetDetails(int senderId)
        {
            return (from x in DataContext.Get<NotificationSender>()
                    join y in DataContext.Get<MailServerProvider>() on x.MailServerProviderId equals y.MailServerProviderId
                    where x.NotificationSenderId == senderId
                    select new NotificationSenderInfo
                    {
                        NotificationSenderId = x.NotificationSenderId,
                        MailServerProviderId = x.MailServerProviderId,
                        SenderName = x.SenderName,
                        ContactName = x.ContactName,
                        MailAddress = x.MailAddress,
                        MailAccount = x.MailAddress.Split('@')[0],
                        Password = x.Password,
                        Signature = x.Signature,
                        IsActive = x.IsActive,
                        MailServerProvider = y
                    }).FirstOrDefault();
        }

        public SelectList PopulateNotificationSenderSelectList(int? mailServerProviderId, int? selectedValue = null, bool? isShowSelectText = true)
        {
            var lst = (from a in DataContext.Get<NotificationSender>()
                join p in DataContext.Get<MailServerProvider>() on a.MailServerProviderId equals p.MailServerProviderId into apJoin
                from provider in apJoin.DefaultIfEmpty()
                where mailServerProviderId == null || a.MailServerProviderId == mailServerProviderId
                orderby a.NotificationSenderId
                select new SelectListItem
                {
                    Text = a.SenderName,
                    Value = a.NotificationSenderId.ToString(),
                    Selected = selectedValue != null && a.MailServerProviderId == selectedValue
                }).ToList();
            
            if (lst.Any())
            {
                if (isShowSelectText != null && isShowSelectText == true)
                    lst.Insert(0, new SelectListItem { Text = $@"--- {LanguageResource.SelectMailServerProvider} ---", Value = "" });
            }
            else
            {
                lst.Insert(0, new SelectListItem { Text = $@"-- {LanguageResource.None} --", Value = "-1" });
            }
            return new SelectList(lst, "Value", "Text", selectedValue);
        }

        public bool HasSenderNameExisted(string senderName)
        {
            var query = DataContext.Get<NotificationSender>().FirstOrDefault(p => p.SenderName.Equals(senderName));
            return (query != null);
        }

        public bool HasMailAddressExisted(string mailAddress)
        {
            var query = DataContext.Get<NotificationSender>().FirstOrDefault(p => p.MailAddress.ToLower().Equals(mailAddress.ToLower()));
            return (query != null);
        }

    }
}
