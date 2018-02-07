using System.Collections.Generic;
using System.Web.Mvc;
using Eagle.Core.Settings;
using Eagle.Entities.Business.Transactions;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.Business
{
    public interface ICurrencyRepository : IRepositoryBase<CurrencyGroup>
    {
        IEnumerable<CurrencyGroup> GetCurrencies(string searchText, CurrencyStatus? status, ref int? recordCount,
            string orderBy = null, int? page = null, int? pageSize = null);

        IEnumerable<CurrencyGroup> GetCurrencyList(CurrencyStatus? status);

        bool HasDataExisted(string currencyName);
        CurrencyGroup GetDetail(string currencyCode);
        CurrencyGroup GetSelectedCurrency();
        SelectList PopulateCurrencySelectList(CurrencyStatus? status = null, string selectedValue = null,
            bool? isShowSelectText = false);
    }
}
