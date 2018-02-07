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
    public class ServicePackDurationRepository : RepositoryBase<ServicePackDuration>, IServicePackDurationRepository
    {
        public ServicePackDurationRepository(IDataContext dataContext) : base(dataContext) { }

        public IEnumerable<ServicePackDuration> GetServicePackDurations(string durationName, bool? status, ref int? recordCount,
            string orderBy = null, int? page = null, int? pageSize = null)
        {
            var queryable = from d in DataContext.Get<ServicePackDuration>()
                            where (status == null || d.IsActive == status)
                            select d;
            if (!string.IsNullOrEmpty(durationName))
            {
                queryable = queryable.Where(x => x.DurationName.ToLower().Contains(durationName.ToLower()));
            }
            return queryable.AsEnumerable().WithRecordCount(ref recordCount).WithSortingAndPaging(orderBy, page, pageSize);
        }

        public bool HasDurationNameExisted(string durationName)
        {
            var query = (from d in DataContext.Get<ServicePackDuration>()
                         where d.DurationName.ToLower().Equals(durationName, StringComparison.OrdinalIgnoreCase)
                         select d).FirstOrDefault();
            return query != null;
        }

        public bool HasAllotedTimeExisted(int allotedTime)
        {
            var query = (from d in DataContext.Get<ServicePackDuration>()
                         where d.AllotedTime == allotedTime
                         select d).FirstOrDefault();
            return query != null;
        }

        public SelectList PopulateServicePackDurationSelectList(bool? status = null, int? selectedValue = null,
           bool? isShowSelectText = true)
        {
            var listItems = new List<SelectListItem>();
            var lst = (from c in DataContext.Get<ServicePackDuration>()
                       where status == null || c.IsActive == status
                       orderby c.AllotedTime
                       select c).ToList();

            if (lst.Any())
            {
                listItems = lst.Select(p => new SelectListItem
                {
                    Text = p.DurationName,
                    Value = p.DurationId.ToString(),
                    Selected = (selectedValue != null && p.DurationId == selectedValue)
                }).ToList();

                if (isShowSelectText != null && isShowSelectText == true)
                {
                    listItems.Insert(0, new SelectListItem { Text = $"--- {LanguageResource.SelectDuration} ---", Value = "" });
                }
            }
            else
            {
                listItems.Insert(0, new SelectListItem { Text = $"-- {LanguageResource.None} --", Value = "-1" });
            }
            return new SelectList(listItems, "Value", "Text", selectedValue);
        }

        public SelectList PopulateServicePackDurationStatus(bool? selectedValue = true, bool isShowSelectText = false)
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

        public ServicePackDuration GetDurationByServicePackId(int ServicePackId)
        {
            try
            {
                var item = (from c in DataContext.Get<ServicePackDuration>()
                            join s in DataContext.Get<ServicePack>() on c.DurationId equals s.DurationId
                            where c.IsActive == true && s.PackageId == ServicePackId
                            select c).FirstOrDefault();

                return item;
            }
            catch
            {
                return null;
            }
        }
    }
}
