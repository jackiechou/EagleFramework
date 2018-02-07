using Eagle.Entities.Business.Orders;
using Eagle.EntityFramework.Repositories;
using System;
using System.Collections.Generic;

namespace Eagle.Repositories.Business
{
    public interface IOrderTempRepository : IRepositoryBase<OrderTemp>
    {
        OrderTempInfo GetOrderTemp(string orderNo);
        OrderTemp FindByOrderNo(Guid orderNo);
        bool DeleteByOrderNo(Guid orderNo);
    }
}
