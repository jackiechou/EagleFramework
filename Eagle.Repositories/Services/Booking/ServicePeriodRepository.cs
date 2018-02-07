using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Eagle.Core.Common;
using Eagle.Entities.Services.Booking;
using Eagle.EntityFramework;
using Eagle.Resources;

namespace Eagle.Repositories.Services.Booking
{
    public class ServicePeriodRepository : RepositoryBase<ServicePeriod>, IServicePeriodRepository
    {
        public ServicePeriodRepository(IDataContext dataContext) : base(dataContext) { }

        public IEnumerable<ServicePeriod> GetPeriods(bool? status, ref int? recordCount,
          string orderBy = null, int? page = null, int? pageSize = null)
        {
            var query = from d in DataContext.Get<ServicePeriod>()
                            where (status == null || d.Status == status)
                            select d;
            return query.WithRecordCount(ref recordCount).WithSortingAndPaging(orderBy, page, pageSize);
        }

        public bool HasDataExisted(string periodName)
        {
            var query = (from d in DataContext.Get<ServicePeriod>()
                         where d.PeriodName.ToLower().Equals(periodName, StringComparison.OrdinalIgnoreCase)
                         select d).FirstOrDefault();
            return query != null;
        }

        public SelectList PopulatePeriodSelectList(bool? status = null, int? selectedValue = null,
          bool? isShowSelectText = true)
        {
            var listItems = new List<SelectListItem>();
            var lst = (from c in DataContext.Get<ServicePeriod>()
                       where status == null || c.Status == status
                       select c).ToList();

            if (lst.Any())
            {
                listItems = lst.Select(p => new SelectListItem
                {
                    Text = p.PeriodName,
                    Value = p.PeriodValue.ToString(),
                    Selected = (selectedValue != null && p.PeriodValue == selectedValue)
                }).ToList();

                if (isShowSelectText != null && isShowSelectText == true)
                {
                    listItems.Insert(0, new SelectListItem { Text = $"--- {LanguageResource.SelectPeriod} ---", Value = "" });
                }
            }
            else
            {
                listItems.Insert(0, new SelectListItem { Text = $"-- {LanguageResource.None} --", Value = "-1" });
            }
            return new SelectList(listItems, "Value", "Text", selectedValue);
        }

        public SelectList PopulatePeriodStatus(bool? selectedValue = true, bool isShowSelectText = false)
        {
            List<SelectListItem> lst = new List<SelectListItem>
            {
                new SelectListItem {Text = LanguageResource.Active, Value = "True", Selected = (selectedValue!=null && selectedValue==true) },
                new SelectListItem {Text = LanguageResource.InActive, Value = "False", Selected = (selectedValue!=null && selectedValue==false) }
            };

            if (isShowSelectText)
                lst.Insert(0, new SelectListItem { Text = $"--- {LanguageResource.SelectStatus} ---", Value = "" });
            return new SelectList(lst, "Value", "Text", selectedValue);
        }

    }
}
