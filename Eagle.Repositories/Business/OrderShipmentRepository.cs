using System;
using System.Collections.Generic;
using System.Linq;
using Eagle.Entities.Business.Orders;
using Eagle.EntityFramework;

namespace Eagle.Repositories.Business
{
    public class OrderShipmentRepository: RepositoryBase<OrderShipment>, IOrderShipmentRepository
    {
        public OrderShipmentRepository(IDataContext dataContext) : base(dataContext) { }
        public IEnumerable<OrderShipment> GetListByOrderNo(Guid orderNo)
        {
            return (from o in DataContext.Get<OrderShipment>()
                    where o.OrderNo == orderNo
                    select o).AsEnumerable();
        }

        public OrderShipment GetDetails(Guid orderNo, int shippingMethodId)
        {
            return (from pm in DataContext.Get<OrderShipment>()
                    where pm.OrderNo == orderNo && pm.ShippingMethodId == shippingMethodId
                    select pm).FirstOrDefault();
        }

        public bool HasOrderProductExisted(int customerId, Guid orderNo, int shippingMethodId)
        {
            var item = (from op in DataContext.Get<OrderShipment>()
                        join o in DataContext.Get<Order>() on op.OrderNo equals o.OrderNo
                        where o.OrderNo == orderNo && op.ShippingMethodId == shippingMethodId && o.CustomerId == customerId
                        select op).FirstOrDefault();
            return item != null;
        }
    }
}
