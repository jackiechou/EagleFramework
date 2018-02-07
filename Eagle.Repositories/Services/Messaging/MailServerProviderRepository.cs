using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Eagle.Core.Common;
using Eagle.Entities.Services.Messaging;
using Eagle.EntityFramework;
using Eagle.Resources;

namespace Eagle.Repositories.Services.Messaging
{
    public class MailServerProviderRepository : RepositoryBase<MailServerProvider>, IMailServerProviderRepository
    {
        public MailServerProviderRepository(IDataContext dataContext) : base(dataContext)
        {
        }

        public IEnumerable<MailServerProvider> GetMailServerProviders(out int recordCount, string orderBy = null, int? page = null, int? pageSize = null)
        {
            return DataContext.Get<MailServerProvider>().WithRecordCount(out recordCount).WithSortingAndPaging(orderBy, page, pageSize);
        }

        public bool HasDataExisted(string providerName)
        {
            var query = DataContext.Get<MailServerProvider>().FirstOrDefault(p => p.MailServerProviderName.Equals(providerName));
            return (query != null);
        }

        public SelectList PopulateMailServerProviderSelectList(int? selectedValue = null, bool? isShowSelectText = true)
        {
            var lst = (from p in DataContext.Get<MailServerProvider>()
                       select new SelectListItem { Text = p.MailServerProviderName, Value = p.MailServerProviderId.ToString(), Selected = (selectedValue != null && p.MailServerProviderId == selectedValue) }).ToList();

            if (lst.Any())
            {
                if (isShowSelectText != null && isShowSelectText == true)
                    lst.Insert(0, new SelectListItem { Text = $"--- {LanguageResource.SelectMailServerProvider} ---", Value = "" });
            }
            else
            {
                lst.Insert(0, new SelectListItem { Text = $"-- {LanguageResource.None} --", Value = "-1" });
            }
            return new SelectList(lst, "Value", "Text", selectedValue);
        }
    }
}
