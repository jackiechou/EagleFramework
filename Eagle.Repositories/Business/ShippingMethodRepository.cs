using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Eagle.Core.Common;
using Eagle.Entities.Business.Shipping;
using Eagle.EntityFramework;
using Eagle.Resources;

namespace Eagle.Repositories.Business
{
    public class ShippingMethodRepository: RepositoryBase<ShippingMethod>, IShippingMethodRepository
    {
        public ShippingMethodRepository(IDataContext dataContext) : base(dataContext) { }

        public IEnumerable<ShippingMethod> GetShippingMethods(string shippingMethodName, bool? status, ref int? recordCount, string orderBy = null, int? page = null, int? pageSize = null)
        {
            var queryable = (from t in DataContext.Get<ShippingMethod>()
                             where (status == null || t.IsActive == status)
                             select t);

            if (!string.IsNullOrEmpty(shippingMethodName))
            {
                queryable = queryable.Where(x => x.ShippingMethodName.ToLower().Contains(shippingMethodName.ToLower()));
            }
            return queryable.AsEnumerable().WithRecordCount(ref recordCount).WithSortingAndPaging(orderBy, page, pageSize);
        }

        public IEnumerable<ShippingMethod> GetShippingMethods(bool? status)
        {
            return (from t in DataContext.Get<ShippingMethod>()
                where (status == null || t.IsActive == status)
                select t).AsEnumerable();
        }

        public int GetNewListOrder()
        {
            int listOrder = 1;
            var query = from u in DataContext.Get<ShippingMethod>() select (int)u.ListOrder;
            if (query.Any())
            {
                listOrder = query.Max() + 1;
            }
            return listOrder;
        }

        public bool HasDataExisted(string shippingMethodName)
        {
            if (string.IsNullOrEmpty(shippingMethodName)) return false;
            var query = DataContext.Get<ShippingMethod>().FirstOrDefault(c => c.ShippingMethodName.Equals(shippingMethodName, StringComparison.OrdinalIgnoreCase));
            return (query != null);
        }

        public SelectList PopulateShippingMethodStatus(bool? selectedValue = true, bool isShowSelectText = false)
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

        public SelectList PopulateShippingMethodSelectList(bool? status = null, int? selectedValue = null, bool? isShowSelectText = true)
        {
            var listItems = new List<SelectListItem>();
            var lst = (from c in DataContext.Get<ShippingMethod>()
                       where (status == null || c.IsActive == status)
                       orderby c.ListOrder ascending
                       select c).ToList();

            if (lst.Any())
            {
                listItems = lst.Select(p => new SelectListItem { Text = p.ShippingMethodName, Value = p.ShippingMethodId.ToString(), Selected = (selectedValue != null && p.ShippingMethodId == selectedValue) }).ToList();
                if (isShowSelectText != null && isShowSelectText == true)
                    listItems.Insert(0, new SelectListItem { Text = $"--- {LanguageResource.SelectShippingMethod} ---", Value = "" });
            }
            else
            {
                listItems.Insert(0, new SelectListItem { Text = $"-- {LanguageResource.None} --", Value = "-1" });
            }
            return new SelectList(listItems, "Value", "Text", selectedValue);
        }
    }
}
