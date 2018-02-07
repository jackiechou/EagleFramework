using Eagle.Entities.Business.Orders;
using Eagle.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using Eagle.Core.Common;
using Eagle.Core.Settings;
using Eagle.Entities.Business.Customers;
using Eagle.Entities.Business.Transactions;

namespace Eagle.Repositories.Business
{
    public class OrderTempRepository : RepositoryBase<OrderTemp>, IOrderTempRepository
    {
        public OrderTempRepository(IDataContext dataContext) : base(dataContext) { }

        public OrderTempInfo GetOrderTemp(string orderNo)
        {
            var orderNoGuid = Guid.Parse(orderNo);
            var lst = (from o in DataContext.Get<OrderTemp>()
                join c in DataContext.Get<Customer>() on o.CustomerId equals c.CustomerId into custList
                from customer in custList.DefaultIfEmpty()
                join op in DataContext.Get<OrderPayment>() on o.OrderNo equals op.OrderNo into opList
                from orderPayment in opList.DefaultIfEmpty()
                join p in DataContext.Get<PaymentMethod>() on orderPayment.PaymentMethodId equals p.PaymentMethodId into pmList
                from paymentMethod in pmList.DefaultIfEmpty()
                join s in DataContext.Get<OrderShipment>() on o.OrderNo equals s.OrderNo into smList
                from shipment in smList.DefaultIfEmpty()
                join t in DataContext.Get<TransactionMethod>() on o.TransactionMethodId equals t.TransactionMethodId into tmList
                from transactionMethod in tmList.DefaultIfEmpty()
                where o.OrderNo == orderNoGuid
                       select new OrderTempInfo
                {
                    OrderId = o.OrderId,
                    OrderNo = o.OrderNo,
                    Customer = customer,
                    OrderPayment = orderPayment,
                    PaymentMethod = paymentMethod,
                    Shipment = shipment,
                    TransactionMethod = transactionMethod
                });
            return lst.FirstOrDefault();
        }
        public OrderTemp FindByOrderNo(Guid orderNo)
        {
            return DataContext.Get<OrderTemp>().FirstOrDefault(x => x.OrderNo == orderNo);
        }
        public bool DeleteByOrderNo(Guid orderNo)
        {
            var result = true;
            var tempOrder = DataContext.Get<OrderTemp>().FirstOrDefault(p => p.OrderNo == orderNo);
            if (tempOrder != null)
            {
                DataContext.Delete(tempOrder);
                result = DataContext.SaveChanges() > 0;
            }
            return result;
        }
    }
}
