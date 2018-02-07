using System;
using System.Collections.Generic;
using Eagle.Entities.Business.Orders;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.Business
{
    public interface IOrderShipmentRepository: IRepositoryBase<OrderShipment>
    {
        IEnumerable<OrderShipment> GetListByOrderNo(Guid orderNo);
        OrderShipment GetDetails(Guid orderNo, int shippingMethodId);
        bool HasOrderProductExisted(int customerId, Guid orderNo, int shippingMethodId);
    }
}
