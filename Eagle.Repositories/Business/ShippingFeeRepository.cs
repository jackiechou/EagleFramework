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
    public class ShippingFeeRepository : RepositoryBase<ShippingFee>, IShippingFeeRepository
    {
        public ShippingFeeRepository(IDataContext dataContext) : base(dataContext) { }

        public IEnumerable<ShippingFee> GetShippingFees(string shippingFeeName, bool? status, ref int? recordCount, string orderBy = null, int? page = null, int? pageSize = null)
        {
            var queryable = (from t in DataContext.Get<ShippingFee>()
                             where (status == null || t.IsActive == status)
                             select t);

            if (!string.IsNullOrEmpty(shippingFeeName))
            {
                queryable = queryable.Where(x => x.ShippingFeeName.ToLower().Contains(shippingFeeName.ToLower()));
            }
            return queryable.AsEnumerable().WithRecordCount(ref recordCount).WithSortingAndPaging(orderBy, page, pageSize);
        }

        public ShippingFee GetShippingFee(int shippingCarrierId, int shippingMethodId, string zipCode, decimal weight)
        {
            return (from p in DataContext.Get<ShippingFee>()
            where p.ShippingCarrierId == shippingCarrierId && p.ShippingMethodId == shippingMethodId
                    && string.CompareOrdinal(p.ZipStart, zipCode) <= 0 && string.CompareOrdinal(p.ZipEnd, zipCode) >= 0
                    && p.WeightStart <= weight && p.WeightEnd >= weight
                    && p.IsActive
            select p).FirstOrDefault();
        }


        public int GetNewListOrder()
        {
            int listOrder = 1;
            var query = from u in DataContext.Get<ShippingFee>() select u.ListOrder;
            if (query.Any())
            {
                listOrder = query.Max() + 1;
            }
            return listOrder;
        }

        public bool HasDataExisted(string shippingFeeName)
        {
            if (string.IsNullOrEmpty(shippingFeeName)) return false;
            var query = DataContext.Get<ShippingFee>().FirstOrDefault(c => c.ShippingFeeName.Equals(shippingFeeName, StringComparison.OrdinalIgnoreCase));
            return (query != null);
        }

        public SelectList PopulateShippingFeeStatus(bool? selectedValue = null, bool isShowSelectText = true)
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

        public IEnumerable<int> GetShippingCarrierProvicedService(int shippingMethodId, bool? status)
        {
            var query = DataContext.Get<ShippingFee>().Where(x => x.ShippingMethodId == shippingMethodId
                                                        && (status == null || status.Value.Equals(true)))
                                                        .Select(p => p.ShippingCarrierId).Distinct();
            return query;
        }
    }
}
