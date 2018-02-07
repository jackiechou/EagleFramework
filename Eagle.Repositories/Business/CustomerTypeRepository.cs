using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Eagle.Core.Common;
using Eagle.Core.Settings;
using Eagle.Entities.Business.Customers;
using Eagle.EntityFramework;
using Eagle.Resources;

namespace Eagle.Repositories.Business
{
    public class CustomerTypeRepository: RepositoryBase<CustomerType>, ICustomerTypeRepository
    {
        public CustomerTypeRepository(IDataContext dataContext) : base(dataContext) { }
        public IEnumerable<CustomerType> GetCustomerTypes(int vendorId, string customerTypeName, CustomerTypeStatus? status, ref int? recordCount, string orderBy = null, int? page = null, int? pageSize = null)
        {
            var query = DataContext.Get<CustomerType>().Where(x => 
            x.VendorId == vendorId && (status == null || x.IsActive == status));
            return query.WithRecordCount(ref recordCount)
                            .WithSortingAndPaging(orderBy, page, pageSize);
        }

        public IEnumerable<CustomerType> GetCustomerTypes(int vendorId, CustomerTypeStatus? status) { 
            return DataContext.Get<CustomerType>().Where(x =>
            x.VendorId == vendorId && (status == null || x.IsActive == status)).AsEnumerable();
        }
        public SelectList PopulateCustomerTypeSelectList(CustomerTypeStatus? status = null, int? selectedValue = null, bool? isShowSelectText = false)
        {
            var listItems = new List<SelectListItem>();
            var lst = (from c in DataContext.Get<CustomerType>()
                       where status == null || c.IsActive == status
                       select c).ToList();

            if (lst.Any())
            {
                listItems = lst.Select(p => new SelectListItem { Text = p.CustomerTypeName, Value = p.CustomerTypeId.ToString(), Selected = (selectedValue != null && p.CustomerTypeId == selectedValue) }).ToList();
                if (isShowSelectText != null && isShowSelectText == true)
                    listItems.Insert(0, new SelectListItem { Text = $"--- {LanguageResource.SelectCustomerType} ---", Value = "" });
            }
            else
            {
                listItems.Insert(0, new SelectListItem { Text = $"-- {LanguageResource.None} --", Value = "-1" });
            }
            return new SelectList(listItems, "Value", "Text", selectedValue);
        }

        public bool HasDataExisted(string customerTypeName)
        {
            var entity =
                DataContext.Get<CustomerType>().FirstOrDefault(x => x.CustomerTypeName.Contains(customerTypeName));
            return entity != null;
        }
    }
}
