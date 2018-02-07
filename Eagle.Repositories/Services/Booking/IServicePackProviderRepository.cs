using System.Collections.Generic;
using System.Web.Mvc;
using Eagle.Core.Settings;
using Eagle.Entities.Business.Employees;
using Eagle.Entities.Services.Booking;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.Services.Booking
{
    public interface IServicePackProviderRepository : IRepositoryBase<ServicePackProvider>
    {
        IEnumerable<ServicePackProvider> GetServicePackProviders(int packageId);
        IEnumerable<EmployeeInfo> GetServicePackProviders(int packageId, EmployeeStatus? status);
        ServicePackProvider GetDetails(int packageId, int employeeId);

        SelectList PopulateProviderSelectList(int packageId, EmployeeStatus? status = null, int? selectedValue = null, bool? isShowSelectText = true);
        MultiSelectList PopulateAvailableProviderMultiSelectList(int? packageId = null,
            EmployeeStatus? status = null, string[] selectedValues = null);

        MultiSelectList PopulateSelectedProviderMultiSelectList(int? packageId = null, EmployeeStatus? status = null,
            string[] selectedValues = null);

        bool HasDataExisted(int packageId, int employeeId);
    }
}
