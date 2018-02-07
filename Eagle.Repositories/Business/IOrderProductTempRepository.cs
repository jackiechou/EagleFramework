using Eagle.Entities.Business.Orders;
using Eagle.EntityFramework.Repositories;
using System;
using System.Collections.Generic;
using Eagle.Core.Settings;

namespace Eagle.Repositories.Business
{
    public interface IOrderProductTempRepository : IRepositoryBase<OrderProductTemp>
    {
        IEnumerable<OrderProductTempInfo> GetOrderProductTempsByOrderNo(string orderNo,
            OrderProductTempStatus? status = null);
        bool HasOrderProductExisted(Guid orderNo, int productId);
        OrderProductTemp GetDetails(Guid orderNo, int productId);
        IEnumerable<OrderProductTemp> GetListByOrderNo(Guid orderNo);
        bool HasOrderProductTempExisted(int productId, ItemType itemType);
    }
}
