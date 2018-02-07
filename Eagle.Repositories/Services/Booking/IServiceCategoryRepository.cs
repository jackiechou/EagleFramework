using System.Collections.Generic;
using Eagle.Core.Settings;
using Eagle.Entities.Common;
using Eagle.Entities.Services.Booking;
using Eagle.EntityFramework.Repositories;
using System.Web.Mvc;

namespace Eagle.Repositories.Services.Booking
{
    public interface IServiceCategoryRepository : IRepositoryBase<ServiceCategory>
    {
        IEnumerable<TreeEntity> GetServiceCategorySelectTree(ServiceType typeId = ServiceType.Single, ServiceCategoryStatus? status = null, int? selectedId = null, bool? isRootShowed = false);

        SelectList GetServiceCategoryChildList(ServiceCategoryStatus? status = null, int? selectedValue = null, bool? isShowSelectText = true);
        SelectList GetServiceCategoryChildListByCode(string discountCode, ServiceCategoryStatus? status = null, int? selectedValue = null, bool? isShowSelectText = true);

        IEnumerable<ServiceCategory> GetAllChildrenNodesOfSelectedNode(int id, ServiceCategoryStatus? status = null);
        IEnumerable<ServiceCategory> GetListByStatus(ServiceCategoryStatus? status);
        bool HasDataExisted(string name, int? parentId);
        bool HasChild(int typeId);
    }
}
