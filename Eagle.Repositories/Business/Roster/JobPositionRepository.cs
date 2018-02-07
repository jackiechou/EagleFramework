using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Eagle.Core.Common;
using Eagle.Entities.Business.Employees;
using Eagle.EntityFramework;
using Eagle.Repositories.Business.Employees;
using Eagle.Resources;

namespace Eagle.Repositories.Business.Roster
{
    public class JobPositionRepository : RepositoryBase<JobPosition>, IJobPositionRepository
    {
        public JobPositionRepository(IDataContext dataContext) : base(dataContext) { }
        public IEnumerable<JobPosition> GetJobPositions(string searchText, bool? status, ref int? recordCount, string orderBy = null, int? page = null, int? pageSize = null)
        {
            var lst = (from c in DataContext.Get<JobPosition>()
                       where (status == null || c.IsActive == status) || c.PositionName.Contains(searchText)
                       select c);
            return lst.WithRecordCount(ref recordCount).WithSortingAndPaging(orderBy, page, pageSize);
        }

        public IEnumerable<JobPosition> GetJobPositions(bool? status)
        {
            return (from c in DataContext.Get<JobPosition>()
                       where (status == null || c.IsActive == status)
                       select c).AsEnumerable();
        }

        public SelectList PopulateJobPositionSelectList(bool? status = null, int? selectedValue = null, bool? isShowSelectText = true)
        {
            var listItems = new List<SelectListItem>();
            var lst = (from c in DataContext.Get<JobPosition>()
                       where status == null || c.IsActive == status
                       select c).ToList();

            if (lst.Any())
            {
                listItems = lst.Select(p => new SelectListItem { Text = p.PositionName, Value = p.PositionId.ToString(), Selected = (selectedValue != null && p.PositionId == selectedValue) }).ToList();
                if (isShowSelectText != null && isShowSelectText == true)
                    listItems.Insert(0, new SelectListItem { Text = $"--- {LanguageResource.SelectJobPosition} ---", Value = "" });
            }
            else
            {
                listItems.Insert(0, new SelectListItem { Text = $"-- {LanguageResource.None} --", Value = "-1" });
            }
            return new SelectList(listItems, "Value", "Text", selectedValue);
        }

        public SelectList PopulateJobPositionStatus(bool? selectedValue = true, bool isShowSelectText = false)
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

        public bool HasDataExisted(string positionName)
        {
            var entity =
                DataContext.Get<JobPosition>().FirstOrDefault(x => x.PositionName.Equals(positionName, StringComparison.OrdinalIgnoreCase));
            return entity != null;
        }
    }
}
