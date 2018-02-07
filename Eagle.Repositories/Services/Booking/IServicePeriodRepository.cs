using System.Collections.Generic;
using System.Web.Mvc;
using Eagle.Entities.Services.Booking;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.Services.Booking
{
    public interface IServicePeriodRepository : IRepositoryBase<ServicePeriod>
    {
        IEnumerable<ServicePeriod> GetPeriods(bool? status, ref int? recordCount,
            string orderBy = null, int? page = null, int? pageSize = null);

        bool HasDataExisted(string periodName);

        SelectList PopulatePeriodSelectList(bool? status = null, int? selectedValue = null,
            bool? isShowSelectText = true);

        SelectList PopulatePeriodStatus(bool? selectedValue = true, bool isShowSelectText = false);
    }
}
