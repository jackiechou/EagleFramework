using System.Collections.Generic;
using System.Web.Mvc;
using Eagle.Core.Settings;
using Eagle.Entities.Business.Customers;
using Eagle.Entities.Common;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.Business
{
    public interface ICustomerRepository: IRepositoryBase<Customer>
    {
        IEnumerable<CustomerInfo> GetCustomers(CustomerStatus? status);
        IEnumerable<Customer> GetCustomers(string searchText, int? customerTypeId, CustomerStatus? status, ref int? recordCount,
           string orderBy = null, int? page = null, int? pageSize = null);

        IEnumerable<AutoComplete> GetCustomerAutoCompleteList(string searchTerm, CustomerStatus? status,
            out int recordCount, string orderBy = null, int? page = null, int? pageSize = null);
        SelectList PopulateCustomerSelectList(CustomerStatus? status = null, int? selectedValue = null,
            bool? isShowSelectText = true);
        Customer FindByCustomerNo(string customerNo);
        Customer FindByEmail(string email);
        Customer FindByUserAndPassword(string email, string hashedPassword);
        bool HasCustomerNameExisted(string firstName, string lastName);
        bool HasEmailExisted(string email);
        bool HasPhoneExisted(string phone);
        bool HasCustomerNumberExisted(string customerNo);
        string GenerateCode(int maxLetters);
    }
}
