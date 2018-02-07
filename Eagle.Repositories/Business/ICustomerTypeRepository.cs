using System.Collections.Generic;
using System.Web.Mvc;
using Eagle.Core.Settings;
using Eagle.Entities.Business.Customers;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.Business
{
    public interface ICustomerTypeRepository: IRepositoryBase<CustomerType>
    {
        IEnumerable<CustomerType> GetCustomerTypes(int vendorId, string customerTypeName, CustomerTypeStatus? status, ref int? recordCount, string orderBy = null, int? page = null, int? pageSize = null);
        IEnumerable<CustomerType> GetCustomerTypes(int vendorId, CustomerTypeStatus? status);

        SelectList PopulateCustomerTypeSelectList(CustomerTypeStatus? status = null, int? selectedValue = null,
            bool? isShowSelectText = false);
        bool HasDataExisted(string customerTypeName);
    }
}
