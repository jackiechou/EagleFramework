using System;
using System.Collections.Generic;
using System.Linq;
using Eagle.Core.Common;
using Eagle.Core.Settings;
using Eagle.Entities.Business.Customers;
using Eagle.Entities.Business.Orders;
using Eagle.Entities.Business.Transactions;
using Eagle.EntityFramework;

namespace Eagle.Repositories.Business
{
    public class OrderRepository : RepositoryBase<Order>, IOrderRepository
    {
        public OrderRepository(IDataContext dataContext) : base(dataContext) { }

        public IEnumerable<OrderInfo> GetOrders(int vendorId, string searchText, DateTime? startDate, DateTime? endDate, OrderStatus? status, ref int? recordCount, string orderBy = null, int? page = null, int? pageSize = null)
        {
            var query = from o in DataContext.Get<Order>()
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
                where o.VendorId == vendorId && (status == null || o.Status == status)
                select new OrderInfo
                {
                    OrderId = o.OrderId,
                    OrderNo = o.OrderNo,
                    OrderDate = o.OrderDate,
                    ShippingRate = o.ShippingRate,
                    PromotionCode = o.PromotionCode,
                    Deposit = o.Deposit,
                    SubTotal = o.SubTotal,
                    TotalFees = o.TotalFees,
                    Discount = o.Discount,
                    Tax = o.Tax,
                    CurrencyCode = o.CurrencyCode,
                    Comment = o.Comment,
                    MarkAsRead = o.MarkAsRead,
                    Status = o.Status,
                    CreatedDate = o.CreatedDate,
                    LastModifiedDate = o.LastModifiedDate,
                    VendorId = o.VendorId,
                    TransactionMethodId = o.TransactionMethodId,
                    CustomerId = o.CustomerId,
                    Customer = customer,
                    OrderPayment = orderPayment,
                    PaymentMethod = paymentMethod,
                    Shipment = shipment,
                    TransactionMethod = transactionMethod
                };

            if (!string.IsNullOrEmpty(searchText))
            {
                query = query.Where(x => x.OrderNo.ToString().Contains(searchText)
                                         || x.OrderId.ToString().Contains(searchText)
                                         || x.Customer.CustomerNo.Contains(searchText)
                                         || x.Customer.FirstName.Contains(searchText)
                                         || x.Customer.LastName.Contains(searchText)
                );
            }

            if (startDate != null)
            {
                query = query.Where(x => x.CreatedDate >= startDate);
            }

            if (endDate != null)
            {
                query = query.Where(x => x.CreatedDate >= endDate);
            }

            return query.WithRecordCount(ref recordCount).WithSortingAndPaging(orderBy, page, pageSize);
        }

        public IEnumerable<OrderInfo> GetListByCustomerNo(string customerNo, OrderStatus? status, ref int? recordCount,
            string orderBy = null, int? page = null, int? pageSize = null)
        {
            var lst = from o in DataContext.Get<Order>()
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
                where customer.CustomerNo.ToLower() == customerNo.ToLower() && (status == null || o.Status == status)
                select new OrderInfo
                {
                    OrderId = o.OrderId,
                    OrderNo = o.OrderNo,
                    OrderDate = o.OrderDate,
                    ShippingRate = o.ShippingRate,
                    PromotionCode = o.PromotionCode,
                    Deposit = o.Deposit,
                    SubTotal = o.SubTotal,
                    TotalFees = o.TotalFees,
                    Discount = o.Discount,
                    Tax = o.Tax,
                    CurrencyCode = o.CurrencyCode,
                    Comment = o.Comment,
                    MarkAsRead = o.MarkAsRead,
                    Status = o.Status,
                    CreatedDate = o.CreatedDate,
                    LastModifiedDate = o.LastModifiedDate,
                    VendorId = o.VendorId,
                    TransactionMethodId = o.TransactionMethodId,
                    CustomerId = o.CustomerId,
                    Customer = customer,
                    OrderPayment = orderPayment,
                    PaymentMethod = paymentMethod,
                    Shipment = shipment,
                    TransactionMethod = transactionMethod
                };
            return lst.WithRecordCount(ref recordCount).WithSortingAndPaging(orderBy, page, pageSize);
        }

        public IEnumerable<OrderInfo> GetOrdersByCustomer(int vendorId, int customerId, string searchText, OrderStatus? status, ref int? recordCount,
           string orderBy = null, int? page = null, int? pageSize = null)
        {
            var query = (from o in DataContext.Get<Order>()
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
                       where customer.VendorId == vendorId 
                       && customer.CustomerId == customerId 
                       && (status == null || o.Status == status)
                       select new OrderInfo
                       {
                           OrderId = o.OrderId,
                           OrderNo = o.OrderNo,
                           OrderDate = o.OrderDate,
                           ShippingRate = o.ShippingRate,
                           PromotionCode = o.PromotionCode,
                           Deposit = o.Deposit,
                           SubTotal = o.SubTotal,
                           TotalFees = o.TotalFees,
                           Discount = o.Discount,
                           Tax = o.Tax,
                           CurrencyCode = o.CurrencyCode,
                           Comment = o.Comment,
                           MarkAsRead = o.MarkAsRead,
                           Status = o.Status,
                           CreatedDate = o.CreatedDate,
                           LastModifiedDate = o.LastModifiedDate,
                           VendorId = o.VendorId,
                           TransactionMethodId = o.TransactionMethodId,
                           CustomerId = o.CustomerId,
                           Customer = customer,
                           OrderPayment = orderPayment,
                           PaymentMethod = paymentMethod,
                           Shipment = shipment,
                           TransactionMethod = transactionMethod
                       });

            if (!string.IsNullOrEmpty(searchText))
            {
                query = query.Where(o => o.OrderNo.ToString().Contains(searchText)
                                     || o.Customer.CustomerNo.Contains(searchText)
                                     || o.Customer.FirstName.ToLower().Contains(searchText.ToLower())
                                     || o.Customer.LastName.ToLower().Contains(searchText.ToLower())
                                     );
            }
            return query.WithRecordCount(ref recordCount).WithSortingAndPaging(orderBy, page, pageSize);
        }
        public IEnumerable<OrderInfo> GetListByMarkAsRead(MarkAsRead markAsRead, ref int? recordCount,
            string orderBy = null, int? page = null, int? pageSize = null)
        {
            var lst = (from o in DataContext.Get<Order>()
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
                       where o.MarkAsRead == markAsRead
                       select new OrderInfo
                       {
                           OrderId = o.OrderId,
                           OrderNo = o.OrderNo,
                           OrderDate = o.OrderDate,
                           ShippingRate = o.ShippingRate,
                           PromotionCode = o.PromotionCode,
                           Deposit = o.Deposit,
                           SubTotal = o.SubTotal,
                           TotalFees = o.TotalFees,
                           Discount = o.Discount,
                           Tax = o.Tax,
                           CurrencyCode = o.CurrencyCode,
                           Comment = o.Comment,
                           MarkAsRead = o.MarkAsRead,
                           Status = o.Status,
                           CreatedDate = o.CreatedDate,
                           LastModifiedDate = o.LastModifiedDate,
                           VendorId = o.VendorId,
                           TransactionMethodId = o.TransactionMethodId,
                           CustomerId = o.CustomerId,
                           Customer = customer,
                           OrderPayment = orderPayment,
                           PaymentMethod = paymentMethod,
                           Shipment = shipment,
                           TransactionMethod = transactionMethod
                       });
            return lst.WithRecordCount(ref recordCount).WithSortingAndPaging(orderBy, page, pageSize);
        }
        public OrderInfo GetDetailsByOrderNo(Guid orderNo)
        {
            return (from o in DataContext.Get<Order>()
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
                    where o.OrderNo == orderNo
                    select new OrderInfo
                    {
                        OrderId = o.OrderId,
                        OrderNo = o.OrderNo,
                        OrderDate = o.OrderDate,
                        ShippingRate = o.ShippingRate,
                        PromotionCode = o.PromotionCode,
                        Deposit = o.Deposit,
                        SubTotal = o.SubTotal,
                        TotalFees = o.TotalFees,
                        Discount = o.Discount,
                        Tax = o.Tax,
                        CurrencyCode = o.CurrencyCode,
                        Comment = o.Comment,
                        MarkAsRead = o.MarkAsRead,
                        Status = o.Status,
                        CreatedDate = o.CreatedDate,
                        LastModifiedDate = o.LastModifiedDate,
                        VendorId = o.VendorId,
                        TransactionMethodId = o.TransactionMethodId,
                        CustomerId = o.CustomerId,
                        Customer = customer,
                        OrderPayment = orderPayment,
                        PaymentMethod = paymentMethod,
                        Shipment = shipment,
                        TransactionMethod = transactionMethod
                    }).FirstOrDefault();
        }
        public Order FindByOrderNo(Guid orderNo)
        {
            return DataContext.Get<Order>().FirstOrDefault(x => x.OrderNo == orderNo);
        }

    }
}
