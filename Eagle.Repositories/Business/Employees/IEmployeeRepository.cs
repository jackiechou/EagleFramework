using System.Collections.Generic;
using System.Web.Mvc;
using Eagle.Core.Settings;
using Eagle.Entities.Business.Employees;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.Business.Employees
{
    public interface IEmployeeRepository : IRepositoryBase<Employee>
    {
        IEnumerable<EmployeeInfo> GetEmployees(int vendorId, string employeeName, EmployeeStatus? status, ref int? recordCount, string orderBy = null, int? page = null, int? pageSize = null);
        IEnumerable<EmployeeInfo> GetEmployees(int vendorId, EmployeeStatus? status);
        EmployeeInfo GetDetails(int employeedId);
        SelectList PopulateEmployeeSelectList(int vendorId, EmployeeStatus? status = null, int? selectedValue = null,
            bool? isShowSelectText = false);

        MultiSelectList PopulateEmployeeMultiSelectList(int vendorId, EmployeeStatus? status = null,
            int[] selectedValues = null);
        bool HasDataExisted(string employeeName);
        bool HasEmployeeNumberExisted(string employeeNumber);
        bool HasEmailExisted(string email);
        bool HasMobileExisted(string mobile);
        bool HasPhoneExisted(string phone);
        string GenerateCode(int maxLetters);

    }
}
