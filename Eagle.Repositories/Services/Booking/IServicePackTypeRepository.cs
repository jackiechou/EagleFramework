using System.Collections.Generic;
using System.Web.Mvc;
using Eagle.Core.Settings;
using Eagle.Entities.Services.Booking;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.Services.Booking
{
    public interface IServicePackTypeRepository : IRepositoryBase<ServicePackType>
    {
        IEnumerable<ServicePackType> GetServicePackTypes(ServicePackTypeStatus? status);
        bool HasDataExisted(string typeName);

        SelectList PopulateServicePackTypeSelectList(ServicePackTypeStatus? status = null, int? selectedValue = null,
            bool? isShowSelectText = true);
    }
}
