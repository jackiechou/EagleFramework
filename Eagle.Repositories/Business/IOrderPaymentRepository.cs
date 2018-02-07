using System;
using System.Collections.Generic;
using Eagle.Entities.Business.Orders;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.Business
{
    public interface IOrderPaymentRepository: IRepositoryBase<OrderPayment>
    {
        IEnumerable<OrderPayment> GetListByOrderNo(Guid orderNo);
        OrderPayment GetDetails(Guid orderNo, int paymentMethodId);
        bool HasOrderProductExisted(int customerId, Guid orderNo, int paymentMethodId);
    }
}
