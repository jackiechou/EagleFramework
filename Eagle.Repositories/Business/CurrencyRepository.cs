using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Eagle.Core.Common;
using Eagle.Core.Settings;
using Eagle.Entities.Business.Transactions;
using Eagle.EntityFramework;
using Eagle.Resources;

namespace Eagle.Repositories.Business
{
    public class CurrencyRepository : RepositoryBase<CurrencyGroup>, ICurrencyRepository
    {
        public CurrencyRepository(IDataContext dataContext) : base(dataContext) { }
        public IEnumerable<CurrencyGroup> GetCurrencies(string searchText, CurrencyStatus? status, ref int? recordCount,
        string orderBy = null, int? page = null, int? pageSize = null)
        {
            var query = DataContext.Get<CurrencyGroup>().Where(x => (status == null || x.IsActive == status));
            if (!string.IsNullOrEmpty(searchText))
            {
                query = query.Where(x => x.CurrencyCode.ToLower().Contains(searchText.ToLower())
                                 || x.CurrencyName.ToLower().Contains(searchText.ToLower()));
            }
            return query.WithRecordCount(ref recordCount).WithSortingAndPaging(orderBy, page, pageSize);
        }

        public IEnumerable<CurrencyGroup> GetCurrencyList(CurrencyStatus? status)
        {
            var query = DataContext.Get<CurrencyGroup>().Where(x => (status == null || x.IsActive == status));
            return query.AsEnumerable();
        }
        public CurrencyGroup GetDetail(string currencyCode)
        {
            return DataContext.Get<CurrencyGroup>().FirstOrDefault(x => x.CurrencyCode.ToLower() == currencyCode.ToLower());
        }
        public CurrencyGroup GetSelectedCurrency()
        {
            return DataContext.Get<CurrencyGroup>().FirstOrDefault(x => x.IsSelected);
        }
        public bool HasDataExisted(string currencyName)
        {
            var entity =
                DataContext.Get<CurrencyGroup>().FirstOrDefault(x => x.CurrencyName.Contains(currencyName));
            return entity != null;
        }

        public SelectList PopulateCurrencySelectList(CurrencyStatus? status = null, string selectedValue = null, bool? isShowSelectText = false)
        {
            var listItems = new List<SelectListItem>();
            var lst = (from c in DataContext.Get<CurrencyGroup>()
                       where status == null || c.IsActive == status
                       select c).ToList();

            if (lst.Any())
            {
                listItems = lst.Select(p => new SelectListItem { Text = p.CurrencyName, Value = p.CurrencyCode, Selected = !string.IsNullOrEmpty(selectedValue) && p.CurrencyCode == selectedValue }).ToList();
                if (isShowSelectText != null && isShowSelectText == true)
                    listItems.Insert(0, new SelectListItem { Text = $"--- {LanguageResource.Select} ---", Value = "" });
            }
            else
            {
                listItems.Insert(0, new SelectListItem { Text = $"-- {LanguageResource.None} --", Value = "" });
            }
            return new SelectList(listItems, "Value", "Text", selectedValue);
        }
    }
}
