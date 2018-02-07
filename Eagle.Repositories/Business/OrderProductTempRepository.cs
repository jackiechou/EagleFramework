using Eagle.Entities.Business.Orders;
using Eagle.EntityFramework;
using System;
using System.Linq;
using System.Collections.Generic;
using Eagle.Core.Settings;
using Eagle.Entities.Business.Customers;
using Eagle.Entities.Business.Employees;

namespace Eagle.Repositories.Business
{
    public class OrderProductTempRepository : RepositoryBase<OrderProductTemp>, IOrderProductTempRepository
    {
        public OrderProductTempRepository(IDataContext dataContext) : base(dataContext) { }
        public IEnumerable<OrderProductTempInfo> GetOrderProductTempsByOrderNo(string orderNo, OrderProductTempStatus? status = null)
        {
            var query = (from b in DataContext.Get<OrderProductTemp>()
                         join e in DataContext.Get<Employee>() on b.EmployeeId equals e.EmployeeId into beJoin
                         from employee in beJoin.DefaultIfEmpty()
                        join c in DataContext.Get<Customer>() on b.CustomerId equals c.CustomerId into custJoin
                        from customer in custJoin.DefaultIfEmpty()
                         where (status == null || b.Status == status) && (orderNo == null || b.OrderNo == Guid.Parse(orderNo))
                         select new OrderProductTempInfo
                         {
                             OrderProductId = b.OrderProductId,
                             OrderNo = b.OrderNo,
                             TypeId = b.TypeId,
                             ProductId = b.ProductId,
                             CustomerId = b.CustomerId,
                             EmployeeId = b.EmployeeId,
                             Quantity = b.Quantity,
                             NetPrice = b.NetPrice,
                             GrossPrice = b.GrossPrice,
                             TaxRate = b.TaxRate,
                             DiscountRate = b.DiscountRate,
                             Weight = b.Weight,
                             PeriodGroupId = b.PeriodGroupId,
                             StartDate = b.StartDate,
                             EndDate = b.EndDate,
                             CurrencyCode = b.CurrencyCode,
                             Comment = b.Comment,
                             Status = b.Status,
                             CreatedDate = b.CreatedDate,
                             LastModifiedDate = b.LastModifiedDate,
                             Employee = employee
                         });

            return query.AsEnumerable();
        }

        public IEnumerable<OrderProductTemp> GetListByOrderNo(Guid orderNo)
        {
            return (from o in DataContext.Get<OrderProductTemp>()
                where o.OrderNo == orderNo
                select o).AsEnumerable();
        }
        public bool HasOrderProductExisted(Guid orderNo, int productId)
        {
            var item = (from o in DataContext.Get<OrderProduct>()
                        where o.OrderNo == orderNo && o.ProductId == productId
                        select o).FirstOrDefault();
            return item != null;
        }
        public OrderProductTemp GetDetails(Guid orderNo, int productId)
        {
            return DataContext.Get<OrderProductTemp>().FirstOrDefault(o => o.OrderNo == orderNo && o.ProductId == productId);
        }

        public bool HasOrderProductTempExisted(int productId, ItemType itemType)
        {
            return DataContext.Get<OrderProductTemp>().Any(t => t.ProductId == productId && t.TypeId == itemType);
        }
    }
}
