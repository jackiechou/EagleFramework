using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Eagle.Core.Settings;
using Eagle.Entities.Services.Booking;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.Services.Booking
{
    public interface IServiceDiscountRepository : IRepositoryBase<ServiceDiscount>
    {
        IEnumerable<ServiceDiscount> GetServiceDiscounts(DateTime? startDate, DateTime? endDate, ServiceDiscountStatus? status, ref int? recordCount,
           string orderBy = null, int? page = null, int? pageSize = null);

        bool HasDataExisted(int? quantity, decimal rate, bool? isPercent);

        SelectList PopulateServiceDiscountSelectList(ServiceDiscountStatus? status = null, int? selectedValue = null, bool? isShowSelectText = true);
        SelectList PopulateServiceDiscountSelectList(DiscountType type, ServiceDiscountStatus? status = null, int? selectedValue = null, bool? isShowSelectText = true);
    }
}
