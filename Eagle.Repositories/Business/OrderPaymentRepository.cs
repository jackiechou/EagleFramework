using System;
using System.Collections.Generic;
using System.Linq;
using Eagle.Entities.Business.Orders;
using Eagle.EntityFramework;

namespace Eagle.Repositories.Business
{
    public class OrderPaymentRepository: RepositoryBase<OrderPayment>, IOrderPaymentRepository
    {
        public OrderPaymentRepository(IDataContext dataContext) : base(dataContext) { }
        public IEnumerable<OrderPayment> GetListByOrderNo(Guid orderNo)
        {
            return (from o in DataContext.Get<OrderPayment>()
                    where o.OrderNo == orderNo
                    select o).AsEnumerable();
        }

        public OrderPayment GetDetails(Guid orderNo, int paymentMethodId)
        {
            return (from pm in DataContext.Get<OrderPayment>()
                    where pm.OrderNo == orderNo && pm.PaymentMethodId == paymentMethodId
                    select pm).FirstOrDefault();
        }

        public bool HasOrderProductExisted(int customerId, Guid orderNo, int paymentMethodId)
        {
            var item = (from op in DataContext.Get<OrderPayment>()
                        join o in DataContext.Get<Order>() on op.OrderNo equals o.OrderNo
                        where o.OrderNo == orderNo && op.PaymentMethodId == paymentMethodId
                        select op).FirstOrDefault();
            return item != null;
        }
    }
}
