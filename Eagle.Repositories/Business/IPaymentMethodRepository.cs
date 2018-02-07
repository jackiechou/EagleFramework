using System.Collections.Generic;
using Eagle.Core.Settings;
using Eagle.Entities.Business.Transactions;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.Business
{
    public interface IPaymentMethodRepository : IRepositoryBase<PaymentMethod>
    {
        IEnumerable<PaymentMethod> GetPaymentMethods(string paymentMethodName, PaymentMethodStatus? status, ref int? recordCount,
            string orderBy = null, int? page = null, int? pageSize = null);

        IEnumerable<PaymentMethod> GetPaymentMethods(PaymentMethodStatus? status=null);
        bool HasDataExisted(string paymentMethodName);
    }
}
