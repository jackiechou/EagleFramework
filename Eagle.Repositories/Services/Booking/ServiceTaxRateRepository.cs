using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Eagle.Core.Common;
using Eagle.Entities.Services.Booking;
using Eagle.EntityFramework;
using Eagle.Resources;

namespace Eagle.Repositories.Services.Booking
{
    public class ServiceTaxRateRepository : RepositoryBase<ServiceTaxRate>, IServiceTaxRateRepository
    {
        public ServiceTaxRateRepository(IDataContext dataContext) : base(dataContext) { }
        public IEnumerable<ServiceTaxRate> GetList(bool? status, ref int? recordCount, string orderBy = null, int? page = null, int? pageSize = null)
        {
            var lst = (from m in DataContext.Get<ServiceTaxRate>()
                       where (status == null || m.IsActive == status)
                       select m);
            return lst.WithRecordCount(ref recordCount).WithSortingAndPaging(orderBy, page, pageSize);
        }
        public bool HasDataExisted(decimal taxRate, bool isPercent)
        {
            var entity =
                DataContext.Get<ServiceTaxRate>().FirstOrDefault(x => x.TaxRate == taxRate && x.IsPercent == isPercent);
            return entity != null;
        }

        public SelectList PopulateServiceTaxRateSelectList(bool? status = null, int? selectedValue = null, bool? isShowSelectText = true)
        {
            var listItems = new List<SelectListItem>();
            var lst = (from c in DataContext.Get<ServiceTaxRate>()
                       where status == null || c.IsActive == status
                       select c).ToList();

            if (lst.Any())
            {
                listItems = lst.Select(p => new SelectListItem
                {
                    Text = $"{p.TaxRate} {(p.IsPercent ? "%" : "")}",
                    Value = p.TaxRateId.ToString(),
                    Selected = (selectedValue != null && p.TaxRateId == selectedValue)
                }).ToList();

                if (isShowSelectText != null && isShowSelectText == true)
                {
                    listItems.Insert(0, new SelectListItem { Text = $"--- {LanguageResource.SelectTaxRate} ---", Value = "" });
                }
            }
            else
            {
                listItems.Insert(0, new SelectListItem { Text = $"-- {LanguageResource.None} --", Value = "-1" });
            }
            return new SelectList(listItems, "Value", "Text", selectedValue);
        }
    }
}
