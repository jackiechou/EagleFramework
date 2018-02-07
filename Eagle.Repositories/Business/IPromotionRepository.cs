using System;
using System.Collections.Generic;
using Eagle.Core.Settings;
using Eagle.Entities.Business.Transactions;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.Business
{
    public interface IPromotionRepository : IRepositoryBase<Promotion>
    {
        IEnumerable<Promotion> GetPromotions(int vendorId, DateTime? startDate, DateTime? endDate, PromotionStatus? status, ref int? recordCount, string orderBy = null, int? page = null, int? pageSize = null);
        Promotion GetDetails(int vendorId, string promotionCode);
        bool HasPromotionTitleExisted(int vendorId, string promotionTitle);
        bool HasPromotionCodeExisted(int vendorId, string promotionCode);
        string GenerateCode(int maxLength);
        Promotion GetActivePromotion(int vendorId, string promotionCode, DateTime date);
        
    }
}
