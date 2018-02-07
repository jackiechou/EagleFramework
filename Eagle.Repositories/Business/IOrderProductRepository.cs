using System;
using System.Collections.Generic;
using Eagle.Core.Settings;
using Eagle.Entities.Business.Orders;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.Business
{
    public interface IOrderProductRepository : IRepositoryBase<OrderProduct>
    {
        IEnumerable<OrderProduct> GetOrderProducts(int vendorId, ItemType typeId, DateTime? startDate,
            DateTime? endDate, OrderProductStatus? status);
        IEnumerable<OrderProduct> GetOrderProductsByOrderNo(Guid orderNo);
        IEnumerable<OrderProduct> GetOrderProductsByCustomer(int vendorId, int customerId);
        OrderProduct GetDetails(Guid orderNo, int productId);
        bool HasOrderProductExisted(Guid orderNo, int productId);
        bool HasOrderProductExisted(int customerId, Guid orderNo, int productId);
        bool HasOrderProductExisted(int productId, ItemType itemType);
    }
}
