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
    public class ShippingCarrierRepository: RepositoryBase<ShippingCarrier>, IShippingCarrierRepository
    {
        public ShippingCarrierRepository(IDataContext dataContext) : base(dataContext) { }
        public ShippingCarrier GetSelectedShippingCarrier(int vendorId)
        {
            return (from t in DataContext.Get<ShippingCarrier>()
                where t.VendorId == vendorId && t.IsSelected select t).FirstOrDefault();
        }
        public IEnumerable<ShippingCarrier> GetShippingCarriers(int vendorId, string shippingCarrierName, bool? status, ref int? recordCount, string orderBy = null, int? page = null, int? pageSize = null)
        {
            var queryable = (from t in DataContext.Get<ShippingCarrier>()
                             where t.VendorId == vendorId && (status == null || t.IsActive == status)
                             select t);

            if (!string.IsNullOrEmpty(shippingCarrierName))
            {
                queryable = queryable.Where(x => x.ShippingCarrierName.ToLower().Contains(shippingCarrierName.ToLower()));
            }
            return queryable.AsEnumerable().WithRecordCount(ref recordCount).WithSortingAndPaging(orderBy, page, pageSize);
        }
        public IEnumerable<ShippingCarrier> GetShippingCarriers(int vendorId, bool? status)
        {
            var queryable = (from t in DataContext.Get<ShippingCarrier>()
                where t.VendorId == vendorId && (status == null || t.IsActive == status)
                select t);
            return queryable.AsEnumerable();
        }
        public int GetNewListOrder()
        {
            int listOrder = 1;
            var query = from u in DataContext.Get<ShippingCarrier>() select (int)u.ListOrder;
            if (query.Any())
            {
                listOrder = query.Max() + 1;
            }
            return listOrder;
        }

        public bool HasDataExisted(int vendorId, string shippingCarrierName)
        {
            if (string.IsNullOrEmpty(shippingCarrierName)) return false;
            var query = DataContext.Get<ShippingCarrier>().FirstOrDefault(c => c.VendorId == vendorId && c.ShippingCarrierName.Equals(shippingCarrierName, StringComparison.OrdinalIgnoreCase));
            return (query != null);
        }
        public SelectList PopulateShippingCarrierStatus(bool? selectedValue = null, bool isShowSelectText = true)
        {
            List<SelectListItem> lst = new List<SelectListItem>
            {
                 new SelectListItem {Text = LanguageResource.Active, Value = "True", Selected = (selectedValue!=null && selectedValue==true) },
                new SelectListItem {Text = LanguageResource.InActive, Value = "False", Selected = (selectedValue!=null && selectedValue==false) }
            };
            if (isShowSelectText)
                lst.Insert(0, new SelectListItem { Text = $"--- {LanguageResource.SelectStatus} ---", Value = "", Selected = true });
            return new SelectList(lst, "Value", "Text", selectedValue);
        }

        public SelectList PopulateShippingCarrierSelectList(bool? status = null, int? selectedValue = null, bool? isShowSelectText = true)
        {
            var listItems = new List<SelectListItem>();
            var lst = (from c in DataContext.Get<ShippingCarrier>()
                       where (status == null || c.IsActive == status)
                       orderby c.ListOrder ascending 
                       select c).ToList();

            if (lst.Any())
            {
                listItems = lst.Select(p => new SelectListItem { Text = p.ShippingCarrierName, Value = p.ShippingCarrierId.ToString(), Selected = (selectedValue != null && p.ShippingCarrierId == selectedValue) }).ToList();
                if (isShowSelectText != null && isShowSelectText == true)
                    listItems.Insert(0, new SelectListItem { Text = $"--- {LanguageResource.SelectShippingCarrier} ---", Value = "", Selected = true });
            }
            else
            {
                listItems.Insert(0, new SelectListItem { Text = $"-- {LanguageResource.None} --", Value = "-1" });
            }
            return new SelectList(listItems, "Value", "Text", selectedValue);
        }
        public IEnumerable<ShippingCarrier> GetShippingCarriersByIds(IEnumerable<int> ids, bool? status)
        {
            var query = DataContext.Get<ShippingCarrier>().Where(x => ids.Contains(x.ShippingCarrierId)
                                                                    && (status == null || status.Value == x.IsActive));
            return query;
        }
    }
}
