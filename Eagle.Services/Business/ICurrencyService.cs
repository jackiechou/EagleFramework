using System.Collections.Generic;
using System.Web.Mvc;
using Eagle.Core.Settings;
using Eagle.Entities.Business.Transactions;
using Eagle.Entities.SystemManagement;
using Eagle.Services.Dtos.Business;

namespace Eagle.Services.Business
{
    public interface ICurrencyService: IBaseService
    {
        #region Currency
        IEnumerable<CurrencyDetail> GetCurrencies(CurrencySearchEntry filter, ref int? recordCount,
            string orderBy = null, int? page = null, int? pageSize = null);

        SelectList PopulateCurrencySelectList(CurrencyStatus? status = null, string selectedValue = null,
            bool? isShowSelectText = false);

        CurrencyDetail GetSelectedCurrency();
        CurrencyDetail GetCurrencyDetail(int id);
        void InsertCurrency(CurrencyEntry entry);
        void UpdateCurrency(CurrencyEditEntry entry);
        void UpdateSelectedCurrency(int id);
        void SetSelectedCurrency(string currencyCode);
        void UpdateCurrencyStatus(int id, CurrencyStatus status);
        #endregion

        #region Currency

        IEnumerable<CurrencyRateDetail> GetCurrencyRates(ref int? recordCount, string orderBy = null, int? page = null,
            int? pageSize = null);
        CurrencyRateDetail GetCurrencyRateDetail(int id);

        void InsertCurrencyRate(CurrencyRateEntry entry);
        void UpdateCurrencyRate(int id, CurrencyRateEntry entry);


        #endregion
    }
}
