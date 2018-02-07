using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Eagle.Core.Settings;
using Eagle.Services.Dtos.Business;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Dtos.SystemManagement;

namespace Eagle.Services.Business
{
    public interface ICustomerService: IBaseService
    {
        #region Customer

        string GenerateCode(int maxLetters);
        IEnumerable<CustomerInfoDetail> GetCustomers(CustomerStatus? status);
        IEnumerable<CustomerDetail> GetCustomers(CustomerSearchEntry filter, ref int? recordCount,
            string orderBy = null, int? page = null, int? pageSize = null);

        Select2PagedResult GetCustomerAutoCompleteList(string searchTerm, CustomerStatus? status, out int recordCount,
            string orderBy = null, int? page = null);
        SelectList PopulateCustomerSelectList(CustomerStatus? status = null, int? selectedValue = null,
            bool? isShowSelectText = true);

        CustomerInfoDetail GetCustomerInfoDetail(int id);
        CustomerDetail GetCustomerDetail(int id);
        CustomerDetail GetCustomerDetailByCustomerNo(string customerNo);
        CustomerDetail GetCustomerDetailByEmail(string email);
        CustomerInfoDetail SignIn(CustomerLogin login);
        CustomerDetail Register(Guid userId, int vendorId, CustomerRegisterEntry entry);
        bool IsDataExisted(string firstName, string lastName);
        bool IsExistedEmail(string email);
        void InsertCustomer(Guid applicationId, Guid userId, int vendorId, CustomerEntry entry);
        void UpdateCustomer(Guid applicationId, Guid userId, int vendorId, CustomerEditEntry entry);
        void UpdateCustomerStatus(Guid userId, int id, CustomerStatus status);
        void EditProfile(Guid applicationId, Guid userId, int vendorId, CustomerEditEntry entry);
        void ChangePassword(CustomerChangePassword entry);
        void ActivateCustomer(Guid userId, string customerNo);
        void LockCustomerAccount(Guid userId, int customerId);
        #endregion

        #region Customer Type
        IEnumerable<CustomerTypeDetail> GetCustomerTypes(int vendorId, CustomerTypeSearchEntry entry, ref int? recordCount, string orderBy = null,
            int? page = null, int? pageSize = null);

        IEnumerable<CustomerTypeDetail> GetCustomerTypes(int vendorId, CustomerTypeStatus? status);

        SelectList PopulateCustomerTypeSelectList(CustomerTypeStatus? status = null, int? selectedValue = null,
            bool? isShowSelectText = false);
        CustomerTypeDetail GetCustomerTypeDetail(int id);
        void InsertCustomerType(Guid userId, int vendorId, CustomerTypeEntry entry);
        void UpdateCustomerType(Guid userId, CustomerTypeEditEntry entry);
        void UpdateCustomerTypeStatus(Guid userId, int id, CustomerTypeStatus status);
        #endregion
    }
}
