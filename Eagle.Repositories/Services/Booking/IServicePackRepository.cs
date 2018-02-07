using System.Collections.Generic;
using System.Web.Mvc;
using Eagle.Core.Settings;
using Eagle.Entities.Services.Booking;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.Services.Booking
{
    public interface IServicePackRepository : IRepositoryBase<ServicePack>
    {
        IEnumerable<ServicePackInfo> GetServicePacks(int? categoryId, ServicePackStatus? status, ref int? recordCount,
            string orderBy = null, int? page = null, int? pageSize = null);
        IEnumerable<ServicePackInfo> GetServicePacks(string servicePackName, int? categoryId, int? typeId, ServicePackStatus? status, ref int? recordCount,
            string orderBy = null, int? page = null, int? pageSize = null);
        IEnumerable<ServicePackInfo> GetServicePacks(int categoryId, ServicePackStatus? status);
        IEnumerable<ServicePackInfo> GetServicePacks(int typeId, int categoryId, ServicePackStatus? status);
        IEnumerable<ServicePackInfo> GetDiscountedServicePacks(int? categoryId, ServicePackStatus? status,
            ref int? recordCount, string orderBy = null, int? page = null, int? pageSize = null);
        ServicePackInfo GetDetail(int servicePackId);
        bool HasDataExisted(string servicePackName);
        SelectList PopulateServicePackStatus(bool? selectedValue = true, bool isShowSelectText = true);
        SelectList PopulateServicePackSelectList(ServicePackStatus? status = null, int? selectedValue = null,
           bool? isShowSelectText = true);
        SelectList PopulateServicePackSelectListNotCode(ServicePackStatus? status = null, int? selectedValue = null, bool? isShowSelectText = true);
        SelectList PopulateServicePackSelectListByCode(string discountCode, ServicePackStatus? status = null, int? selectedValue = null,
           bool? isShowSelectText = true);
        SelectList PopulateServicePackSelectListByCateId(int categoryId, ServicePackStatus? status = null, int? selectedValue = null, bool? isShowSelectText = true);
        int GetNewListOrder();
    }
}
