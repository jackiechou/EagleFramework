using System;
using System.Collections.Generic;
using Eagle.Core.Settings;
using Eagle.Entities.Business.Orders;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.Business
{
    public interface IOrderRepository : IRepositoryBase<Order>
    {
        IEnumerable<OrderInfo> GetOrders(int vendorId, string searchText, DateTime? startDate, DateTime? endDate, OrderStatus? status, ref int? recordCount, string orderBy = null, int? page = null, int? pageSize = null);
        
        IEnumerable<OrderInfo> GetListByCustomerNo(string customerNo, OrderStatus? status, ref int? recordCount,
            string orderBy = null, int? page = null, int? pageSize = null);

        IEnumerable<OrderInfo> GetOrdersByCustomer(int vendorId, int customerId, string searchText, OrderStatus? status, ref int? recordCount,
            string orderBy = null, int? page = null, int? pageSize = null);
        IEnumerable<OrderInfo> GetListByMarkAsRead(MarkAsRead markAsRead, ref int? recordCount,
            string orderBy = null, int? page = null, int? pageSize = null);
        OrderInfo GetDetailsByOrderNo(Guid orderNo);
        Order FindByOrderNo(Guid orderNo);
    }
}
