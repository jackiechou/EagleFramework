using System.Collections.Generic;
using System.Web.Mvc;
using Eagle.Entities.Services.Messaging;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.Services.Messaging
{
    public interface IMailServerProviderRepository: IRepositoryBase<MailServerProvider>
    {
        IEnumerable<MailServerProvider> GetMailServerProviders(out int recordCount, string orderBy = null, int? page = null, int? pageSize = null);
        bool HasDataExisted(string providerName);
        SelectList PopulateMailServerProviderSelectList(int? selectedValue = null, bool? isShowSelectText = true);
    }
}
