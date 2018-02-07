using System;
using System.Collections.Generic;
using System.Linq;
using Eagle.Core.Settings;
using Eagle.Entities.Business.Orders;
using Eagle.EntityFramework;

namespace Eagle.Repositories.Business
{
    public class OrderProductRepository : RepositoryBase<OrderProduct>, IOrderProductRepository
    {
        public OrderProductRepository(IDataContext dataContext) : base(dataContext) { }

        public IEnumerable<OrderProduct> GetOrderProducts(int vendorId, ItemType typeId, DateTime? startDate, DateTime? endDate, OrderProductStatus? status)
        {
            var query = from op in DataContext.Get<OrderProduct>()
                join o in DataContext.Get<Order>() on op.OrderNo equals o.OrderNo into orderJoin
                from order in orderJoin.DefaultIfEmpty()
                where order.VendorId == vendorId
                      && op.TypeId == typeId
                select op;

            if (startDate != null && endDate == null)
            {
                query = query.Where(x => x.StartDate != null && x.StartDate >= startDate);
            }
            if (startDate == null && endDate != null)
            {
                query = query.Where(x => x.EndDate != null && x.EndDate <= endDate);
            }
            if (startDate != null && endDate != null)
            {
                query = query.Where(x => (x.StartDate != null && x.EndDate != null) && (x.StartDate >= startDate && x.EndDate <= endDate));
            }

            return query.AsEnumerable();
        }

        public IEnumerable<OrderProduct> GetListByOrderNo(Guid orderNo)
        {
            return (from o in DataContext.Get<OrderProduct>()
                    where o.OrderNo == orderNo select o).AsEnumerable();
        }

        public IEnumerable<OrderProduct> GetOrderProductsByOrderNo(Guid orderNo)
        {
            return (from op in DataContext.Get<OrderProduct>()
                        join o in DataContext.Get<Order>() on op.OrderNo equals o.OrderNo into orderJoin
                        from order in orderJoin.DefaultIfEmpty()
                        where order.OrderNo == orderNo
                        select op).AsEnumerable();
        }

        public IEnumerable<OrderProduct> GetOrderProductsByCustomer(int vendorId, int customerId)
        {
            return from op in DataContext.Get<OrderProduct>()
                       join o in DataContext.Get<Order>() on op.OrderNo equals o.OrderNo into orderJoin
                       from order in orderJoin.DefaultIfEmpty()
                       where order.VendorId == vendorId
                       && order.CustomerId == customerId
                       select op;
        }
        public OrderProduct GetDetails(Guid orderNo, int productId)
        {
            return DataContext.Get<OrderProduct>().FirstOrDefault(o => o.OrderNo == orderNo && o.ProductId == productId);
        }

        public bool HasOrderProductExisted(Guid orderNo, int productId)
        {
            var item = (from o in DataContext.Get<OrderProduct>()
                where o.OrderNo == orderNo && o.ProductId == productId
                select o).FirstOrDefault();
            return item != null;
        }

        public bool HasOrderProductExisted(int customerId, Guid orderNo, int productId)
        {
            var item = (from op in DataContext.Get<OrderProductTemp>()
                join o in DataContext.Get<Order>() on op.OrderNo equals o.OrderNo
                where o.OrderNo == orderNo && op.ProductId == productId && o.CustomerId == customerId
                select op).FirstOrDefault();
            return item!=null;
        }

        public bool HasOrderProductExisted(int productId, ItemType itemType)
        {
            var item = (from o in DataContext.Get<OrderProduct>()
                        where o.ProductId == productId && o.TypeId == itemType
                        select o).FirstOrDefault();
            return item != null;
        }
    }
}
