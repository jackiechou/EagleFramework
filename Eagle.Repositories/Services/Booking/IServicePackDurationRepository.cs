using System.Collections.Generic;
using System.Web.Mvc;
using Eagle.Entities.Services.Booking;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.Services.Booking
{
    public interface IServicePackDurationRepository : IRepositoryBase<ServicePackDuration>
    {
        IEnumerable<ServicePackDuration> GetServicePackDurations(string durationName, bool? status, ref int? recordCount,
            string orderBy = null, int? page = null, int? pageSize = null);

        bool HasDurationNameExisted(string durationName);
        bool HasAllotedTimeExisted(int allotedTime);

        SelectList PopulateServicePackDurationSelectList(bool? status = null, int? selectedValue = null,
            bool? isShowSelectText = true);

        SelectList PopulateServicePackDurationStatus(bool? selectedValue = true, bool isShowSelectText = false);

        ServicePackDuration GetDurationByServicePackId(int ServicePackId);
    }
}
