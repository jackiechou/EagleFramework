using System.Collections.Generic;
using System.Web.Mvc;
using Eagle.Entities.Services.Booking;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.Services.Booking
{
    public interface IServiceTaxRateRepository : IRepositoryBase<ServiceTaxRate>
    {
        IEnumerable<ServiceTaxRate> GetList(bool? status, ref int? recordCount, string orderBy = null,
            int? page = null, int? pageSize = null);
        bool HasDataExisted(decimal taxRate, bool isPercent);

        SelectList PopulateServiceTaxRateSelectList(bool? status = null, int? selectedValue = null, bool? isShowSelectText = true);
    }
}
