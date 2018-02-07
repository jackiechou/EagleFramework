using System;
using System.Collections.Generic;
using System.Linq;
using Eagle.Common.Security.Cryptography;
using Eagle.Core.Common;
using Eagle.Core.Settings;
using Eagle.Entities.Business.Transactions;
using Eagle.EntityFramework;

namespace Eagle.Repositories.Business
{
    public class PromotionRepository : RepositoryBase<Promotion>, IPromotionRepository
    {
        public PromotionRepository(IDataContext dataContext) : base(dataContext) { }
        public IEnumerable<Promotion> GetPromotions(int vendorId, DateTime? startDate, DateTime? endDate, PromotionStatus? status, ref int? recordCount, string orderBy = null, int? page = null, int? pageSize = null)
        {
            var query = (from t in DataContext.Get<Promotion>()
                       where t.VendorId == vendorId
                       && (status == null || t.IsActive == status)
                       select t);
            if (startDate != null && endDate != null)
            {
                query = query.Where(x => (startDate == null || x.StartDate == startDate)
                                         && (endDate == null || x.EndDate == endDate));
            }
            return query.WithRecordCount(ref recordCount).WithSortingAndPaging(orderBy, page, pageSize);
        }

        public Promotion GetDetails(int vendorId, string promotionCode)
        {
            if (string.IsNullOrEmpty(promotionCode)) return null;
            return DataContext.Get<Promotion>().FirstOrDefault(x => x.VendorId == vendorId && x.PromotionCode.ToLower() == promotionCode.ToLower());
        }
        public bool HasPromotionTitleExisted(int vendorId, string promotionTitle)
        {
            if (string.IsNullOrEmpty(promotionTitle)) return false;
            var query = DataContext.Get<Promotion>().FirstOrDefault(x => x.VendorId == vendorId && x.PromotionTitle.Equals(promotionTitle, StringComparison.OrdinalIgnoreCase));
            return (query != null);
        }
        public bool HasPromotionCodeExisted(int vendorId, string promotionCode)
        {
            var query = DataContext.Get<Promotion>().Where(x => x.VendorId == vendorId && promotionCode == null || (x.PromotionCode.ToLower() == promotionCode.ToLower()));
            return (query.Any());
        }

        public string GenerateCode(int maxLength)
        {
            int max = maxLength < 6 ? 6 : maxLength;
            return RandomText.Generate(6, max);
        }

        public Promotion GetActivePromotion(int vendorId, string promotionCode, DateTime date)
        {
            Promotion result = null;
            if (!string.IsNullOrEmpty(promotionCode))
            {
                result = DataContext.Get<Promotion>()
                    .FirstOrDefault(x => x.VendorId == vendorId 
                                        && x.PromotionCode.ToLower() == promotionCode.ToLower()
                                        && (x.StartDate.HasValue == false || (x.StartDate.HasValue && x.StartDate.Value <= date))
                                        && (x.EndDate.HasValue == false || (x.EndDate.HasValue && x.EndDate.Value >= date))
                                        && x.IsActive == PromotionStatus.Active);
            }
            return result;
        }

        
    }
}
