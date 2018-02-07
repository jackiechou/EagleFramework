using System.Collections.Generic;
using System.Linq;
using Eagle.Core.Common;
using Eagle.Core.Settings;
using Eagle.Entities.Business.Transactions;
using Eagle.EntityFramework;

namespace Eagle.Repositories.Business
{
    public class PaymentMethodRepository : RepositoryBase<PaymentMethod>, IPaymentMethodRepository
    {
        public PaymentMethodRepository(IDataContext dataContext) : base(dataContext) { }
        public IEnumerable<PaymentMethod> GetPaymentMethods(string paymentMethodName, PaymentMethodStatus? status, ref int? recordCount,
           string orderBy = null, int? page = null, int? pageSize = null)
        {
            var queryable = DataContext.Get<PaymentMethod>().Where(x => (status == null || x.IsActive == status));
            if (!string.IsNullOrEmpty(paymentMethodName))
            {
                queryable =
                    queryable.Where(x => x.PaymentMethodName.ToLower().Contains(paymentMethodName.ToLower()));
            }
            return queryable.AsEnumerable().WithRecordCount(ref recordCount).WithSortingAndPaging(orderBy, page, pageSize);
        }
        public IEnumerable<PaymentMethod> GetPaymentMethods(PaymentMethodStatus? status = null)
        {
            return DataContext.Get<PaymentMethod>().Where(x => status == null || x.IsActive == status).AsEnumerable();
        }
        public bool HasDataExisted(string paymentMethodName)
        {
            var entity =
                DataContext.Get<PaymentMethod>().FirstOrDefault(x => 
                x.PaymentMethodName.Contains(paymentMethodName)
                && x.IsActive == PaymentMethodStatus.Active
                );
            return entity != null;
        }

    }
}
