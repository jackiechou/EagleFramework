using Eagle.Common.Services.Mail;
using Eagle.Common.Services.Parse;
using Eagle.Core.Common;
using Eagle.Core.Permission;
using Eagle.Core.Settings;
using Eagle.Entities.Business.Orders;
using Eagle.Entities.Business.Transactions;
using Eagle.Entities.Services.Messaging;
using Eagle.Repositories;
using Eagle.Resources;
using Eagle.Services.Business.Validation;
using Eagle.Services.Dtos.Business;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Dtos.Services.Message;
using Eagle.Services.EntityMapping.Common;
using Eagle.Services.Messaging;
using Eagle.Services.Validations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using Eagle.Services.Dtos.Business.Transaction;
using Eagle.Services.SystemManagement;
using PayPal.Api;
using Stripe;

namespace Eagle.Services.Business
{
    public class OrderService : BaseService, IOrderService
    {
        public IApplicationService ApplicationService { get; set; }
        public IVendorService VendorService { get; set; }
        public ICustomerService CustomerService { get; set; }
        public IEmployeeService EmployeeService { get; set; }
        public IMailService MailService { get; set; }
        public IMessageService MessageService { get; set; }
        public INotificationService NotificationService { get; set; }
        public IPayPalService PayPalService { get; set; }

        public OrderService(IUnitOfWork unitOfWork, IApplicationService applicationService, IVendorService vendorService, IEmployeeService employeeService,
            ICustomerService customerService, IMailService mailService, IMessageService messageService, 
            INotificationService notificationService, IPayPalService payPalService) : base(unitOfWork)
        {
            ApplicationService = applicationService;
            VendorService = vendorService;
            CustomerService = customerService;
            EmployeeService = employeeService;
            MailService = mailService;
            MessageService = messageService;
            NotificationService = notificationService;
            PayPalService = payPalService;
        }

        public OrderService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        #region Order

        public IEnumerable<OrderInfoDetail> GetOrders(int vendorId, OrderSearchEntry filter, ref int? recordCount,
            string orderBy = null, int? page = null, int? pageSize = null)
        {
            var lst = new List<OrderInfoDetail>();
            var orders = UnitOfWork.OrderRepository.GetOrders(vendorId, filter.SearchText, filter.FromDate, filter.ToDate, filter.Status, ref recordCount, orderBy, page,
                pageSize).ToList();
            if (orders.Any())
            {
                lst.AddRange(orders.Select(item => new OrderInfoDetail
                {
                    OrderId = item.OrderId,
                    OrderNo = item.OrderNo,
                    OrderDate = item.OrderDate,
                    DueDate = item.DueDate,
                    ShippingRate = item.ShippingRate,
                    PromotionCode = item.PromotionCode,
                    Deposit = item.Deposit,
                    SubTotal = item.SubTotal,
                    TotalFees = item.TotalFees,
                    Discount = item.Discount,
                    Tax = item.Tax,
                    CurrencyCode = item.CurrencyCode,
                    Comment = item.Comment,
                    MarkAsRead = item.MarkAsRead,
                    Status = item.Status,
                    CreatedDate = item.CreatedDate,
                    LastModifiedDate = item.LastModifiedDate,
                    VendorId = item.VendorId,
                    TransactionMethodId = item.TransactionMethodId,
                    CustomerId = item.CustomerId,
                    Customer = item.Customer.ToDto<Entities.Business.Customers.Customer, CustomerDetail>(),
                    TransactionMethod = item.TransactionMethod.ToDto<TransactionMethod, TransactionMethodDetail>(),
                    OrderPayment = item.OrderPayment.ToDto<OrderPayment, OrderPaymentDetail>(),
                    Shipment = item.Shipment.ToDto<OrderShipment, OrderShipmentDetail>(),
                    PaymentMethod = item.PaymentMethod.ToDto<PaymentMethod, PaymentMethodDetail>(),
                    OrderProducts = GetOrderProductsByOrderNo(item.OrderNo),
                }));
            }
            return lst;
        }

        public IEnumerable<OrderInfoDetail> GetListByCustomerNo(string customerNo, OrderStatus? status, ref int? recordCount,
            string orderBy = null, int? page = null, int? pageSize = null)
        {
            var lst = new List<OrderInfoDetail>();
            var orders = UnitOfWork.OrderRepository.GetListByCustomerNo(customerNo, status, ref recordCount, orderBy, page,
                 pageSize).ToList();
            if (orders.Any())
            {
                lst.AddRange(orders.Select(item => new OrderInfoDetail
                {
                    OrderId = item.OrderId,
                    OrderNo = item.OrderNo,
                    OrderDate = item.OrderDate,
                    DueDate = item.DueDate,
                    ShippingRate = item.ShippingRate,
                    PromotionCode = item.PromotionCode,
                    Deposit = item.Deposit,
                    SubTotal = item.SubTotal,
                    TotalFees = item.TotalFees,
                    Discount = item.Discount,
                    Tax = item.Tax,
                    CurrencyCode = item.CurrencyCode,
                    Comment = item.Comment,
                    MarkAsRead = item.MarkAsRead,
                    Status = item.Status,
                    CreatedDate = item.CreatedDate,
                    LastModifiedDate = item.LastModifiedDate,
                    VendorId = item.VendorId,
                    TransactionMethodId = item.TransactionMethodId,
                    CustomerId = item.CustomerId,
                    Customer = item.Customer.ToDto<Entities.Business.Customers.Customer, CustomerDetail>(),
                    TransactionMethod = item.TransactionMethod.ToDto<TransactionMethod, TransactionMethodDetail>(),
                    OrderPayment = item.OrderPayment.ToDto<OrderPayment, OrderPaymentDetail>(),
                    Shipment = item.Shipment.ToDto<OrderShipment, OrderShipmentDetail>(),
                    PaymentMethod = item.PaymentMethod.ToDto<PaymentMethod, PaymentMethodDetail>(),
                    OrderProducts = GetOrderProductsByOrderNo(item.OrderNo),
                }));
            }
            return lst;
        }
        public IEnumerable<OrderInfoDetail> GetOrdersByCustomer(OrderSearch filter, ref int? recordCount,
           string orderBy = null, int? page = null, int? pageSize = null)
        {
            var lst = new List<OrderInfoDetail>();
            var orders = UnitOfWork.OrderRepository.GetOrdersByCustomer(filter.VendorId, filter.CustomerId, filter.SearchText, filter.Status, ref recordCount, orderBy, page,
                 pageSize).ToList();

            if (orders.Any())
            {
                lst.AddRange(orders.Select(item => new OrderInfoDetail
                {
                    OrderId = item.OrderId,
                    OrderNo = item.OrderNo,
                    OrderDate = item.OrderDate,
                    DueDate = item.DueDate,
                    ShippingRate = item.ShippingRate,
                    PromotionCode = item.PromotionCode,
                    Deposit = item.Deposit,
                    SubTotal = item.SubTotal,
                    TotalFees = item.TotalFees,
                    Discount = item.Discount,
                    Tax = item.Tax,
                    CurrencyCode = item.CurrencyCode,
                    Comment = item.Comment,
                    MarkAsRead = item.MarkAsRead,
                    Status = item.Status,
                    CreatedDate = item.CreatedDate,
                    LastModifiedDate = item.LastModifiedDate,
                    VendorId = item.VendorId,
                    TransactionMethodId = item.TransactionMethodId,
                    CustomerId = item.CustomerId,
                    Customer = item.Customer.ToDto<Entities.Business.Customers.Customer, CustomerDetail>(),
                    TransactionMethod = item.TransactionMethod.ToDto<TransactionMethod, TransactionMethodDetail>(),
                    OrderPayment = item.OrderPayment.ToDto<OrderPayment, OrderPaymentDetail>(),
                    Shipment = item.Shipment.ToDto<OrderShipment, OrderShipmentDetail>(),
                    PaymentMethod = item.PaymentMethod.ToDto<PaymentMethod, PaymentMethodDetail>(),
                    OrderProducts = GetOrderProductsByOrderNo(item.OrderNo)
                }));
            }
            return lst;
        }
        public IEnumerable<OrderInfoDetail> GetListByMarkAsRead(MarkAsRead markAsRead, ref int? recordCount,
            string orderBy = null, int? page = null, int? pageSize = null)
        {
            var lst = new List<OrderInfoDetail>();
            var orders = UnitOfWork.OrderRepository.GetListByMarkAsRead(markAsRead, ref recordCount, orderBy, page,
                  pageSize).ToList();
            if (orders.Any())
            {
                lst.AddRange(orders.Select(item => new OrderInfoDetail
                {
                    OrderId = item.OrderId,
                    OrderNo = item.OrderNo,
                    OrderDate = item.OrderDate,
                    DueDate = item.DueDate,
                    ShippingRate = item.ShippingRate,
                    PromotionCode = item.PromotionCode,
                    Deposit = item.Deposit,
                    SubTotal = item.SubTotal,
                    TotalFees = item.TotalFees,
                    Discount = item.Discount,
                    Tax = item.Tax,
                    CurrencyCode = item.CurrencyCode,
                    Comment = item.Comment,
                    MarkAsRead = item.MarkAsRead,
                    Status = item.Status,
                    CreatedDate = item.CreatedDate,
                    LastModifiedDate = item.LastModifiedDate,
                    VendorId = item.VendorId,
                    TransactionMethodId = item.TransactionMethodId,
                    CustomerId = item.CustomerId,
                    Customer = item.Customer.ToDto<Entities.Business.Customers.Customer, CustomerDetail>(),
                    TransactionMethod = item.TransactionMethod.ToDto<TransactionMethod, TransactionMethodDetail>(),
                    OrderPayment = item.OrderPayment.ToDto<OrderPayment, OrderPaymentDetail>(),
                    Shipment = item.Shipment.ToDto<OrderShipment, OrderShipmentDetail>(),
                    PaymentMethod = item.PaymentMethod.ToDto<PaymentMethod, PaymentMethodDetail>(),
                    OrderProducts = GetOrderProductsByOrderNo(item.OrderNo),
                }));
            }
            return lst;
        }
        public OrderDetail GetOrderDetail(int id)
        {
            var entity = UnitOfWork.OrderRepository.FindById(id);
            return entity.ToDto<Entities.Business.Orders.Order, OrderDetail>();
        }

        public OrderInfoDetail GetOrderDetailByOrderNo(Guid orderNo)
        {
            var item = UnitOfWork.OrderRepository.GetDetailsByOrderNo(orderNo);
            var orderInfo = new OrderInfoDetail
            {
                OrderId = item.OrderId,
                OrderNo = item.OrderNo,
                OrderDate = item.OrderDate,
                DueDate = item.DueDate,
                ShippingRate = item.ShippingRate,
                PromotionCode = item.PromotionCode,
                Deposit = item.Deposit,
                SubTotal = item.SubTotal,
                TotalFees = item.TotalFees,
                Discount = item.Discount,
                Tax = item.Tax,
                CurrencyCode = item.CurrencyCode,
                Comment = item.Comment,
                MarkAsRead = item.MarkAsRead,
                Status = item.Status,
                CreatedDate = item.CreatedDate,
                LastModifiedDate = item.LastModifiedDate,
                VendorId = item.VendorId,
                TransactionMethodId = item.TransactionMethodId,
                CustomerId = item.CustomerId,
                Customer = item.Customer.ToDto<Entities.Business.Customers.Customer, CustomerDetail>(),
                TransactionMethod = item.TransactionMethod.ToDto<TransactionMethod, TransactionMethodDetail>(),
                OrderPayment = item.OrderPayment.ToDto<OrderPayment, OrderPaymentDetail>(),
                Shipment = item.Shipment.ToDto<OrderShipment, OrderShipmentDetail>(),
                PaymentMethod = item.PaymentMethod.ToDto<PaymentMethod, PaymentMethodDetail>(),
                OrderProducts = GetOrderProductsByOrderNo(item.OrderNo),
            };
            return orderInfo;
        }

        public OrderDetail InsertOrder(int vendorId, OrderEntry entry)
        {
            ISpecification<OrderEntry> validator = new OrderEntryValidator(UnitOfWork, PermissionLevel.Edit, CurrentClaimsIdentity);
            var dataViolations = new List<RuleViolation>();
            var isDataValid = validator.IsSatisfyBy(entry, dataViolations);
            if (!isDataValid) throw new ValidationError(dataViolations);

            var entity = entry.ToEntity<OrderEntry, Entities.Business.Orders.Order>();

            entity.OrderNo = Guid.NewGuid();
            entity.VendorId = vendorId;
            entity.CreatedDate = DateTime.UtcNow;

            UnitOfWork.OrderRepository.Insert(entity);
            UnitOfWork.SaveChanges();

            return entity.ToDto<Entities.Business.Orders.Order, OrderDetail>();
        }

        public void UpdateOrder(int id, OrderEntry entry)
        {
            ISpecification<OrderEntry> validator = new OrderEntryValidator(UnitOfWork, PermissionLevel.Edit, CurrentClaimsIdentity);
            var dataViolations = new List<RuleViolation>();
            var isDataValid = validator.IsSatisfyBy(entry, dataViolations);
            if (!isDataValid) throw new ValidationError(dataViolations);

            var entity = UnitOfWork.OrderRepository.FindById(id);
            if (entity == null)
            {
                dataViolations.Add(new RuleViolation(ErrorCode.NotFoundOrder, "Order"));
                throw new ValidationError(dataViolations);
            }

            entity.CustomerId = entry.CustomerId;
            entity.ShippingRate = entry.ShippingRate;
            entity.PromotionCode = entry.PromotionCode;
            entity.SubTotal = entry.SubTotal;
            entity.TotalFees = entry.TotalFees;
            entity.Discount = entry.Discount;
            entity.Tax = entry.Tax;
            entity.CurrencyCode = entry.CurrencyCode;
            entity.Comment = entry.Comment;
            entity.LastModifiedDate = DateTime.UtcNow;

            UnitOfWork.OrderRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }

        public void UpdateOrder(Guid orderNo, OrderEntry entry)
        {
            ISpecification<OrderEntry> validator = new OrderEntryValidator(UnitOfWork, PermissionLevel.Edit, CurrentClaimsIdentity);
            var dataViolations = new List<RuleViolation>();
            var isDataValid = validator.IsSatisfyBy(entry, dataViolations);
            if (!isDataValid) throw new ValidationError(dataViolations);

            var entity = UnitOfWork.OrderRepository.FindByOrderNo(orderNo);
            if (entity == null)
            {
                dataViolations.Add(new RuleViolation(ErrorCode.NotFoundOrder, "Order"));
                throw new ValidationError(dataViolations);
            }

            entity.CustomerId = entry.CustomerId;
            entity.ShippingRate = entry.ShippingRate;
            entity.PromotionCode = entry.PromotionCode;
            entity.SubTotal = entry.SubTotal;
            entity.TotalFees = entry.TotalFees;
            entity.Discount = entry.Discount;
            entity.Tax = entry.Tax;
            entity.CurrencyCode = entry.CurrencyCode;
            entity.Comment = entry.Comment;
            entity.LastModifiedDate = DateTime.UtcNow;

            UnitOfWork.OrderRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }

        public void UpdateMarkAsRead(Guid orderNo)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.OrderRepository.FindByOrderNo(orderNo);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundOrder, "Order", orderNo));
                throw new ValidationError(violations);
            }

            entity.MarkAsRead = MarkAsRead.Read;
            entity.LastModifiedDate = DateTime.UtcNow;

            UnitOfWork.OrderRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }

        public void UpdateOrderStatus(Guid orderNo, OrderStatus status)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.OrderRepository.FindByOrderNo(orderNo);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundOrder, "Order", orderNo));
                throw new ValidationError(violations);
            }

            var isValid = Enum.IsDefined(typeof(OrderStatus), status);
            if (!isValid)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidStatus, "Status", null,
                    ErrorMessage.Messages[ErrorCode.InvalidStatus]));
                throw new ValidationError(violations);
            }
            if (entity.Status == status) return;

            entity.Status = status;
            entity.LastModifiedDate = DateTime.UtcNow;

            UnitOfWork.OrderRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }

        public void DeleteOrder(int id)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.OrderRepository.FindById(id);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundOrder, "Order", id));
                throw new ValidationError(violations);
            }
            if (entity.Status == OrderStatus.Cancelled) return;

            entity.Status = OrderStatus.Cancelled;
            entity.LastModifiedDate = DateTime.UtcNow;

            UnitOfWork.OrderRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        #endregion

        #region Order Temp

        public OrderTempInfoDetail GetOrderTempByOrderNo(string orderNo)
        {
            var item = UnitOfWork.OrderTempRepository.GetOrderTemp(orderNo);
            if (item == null) return null;

            var customer = CustomerService.GetCustomerInfoDetail(item.CustomerId);
            var orderTemp = new OrderTempInfoDetail
            {
                OrderId = item.OrderId,
                OrderNo = item.OrderNo,
                OrderDate = item.OrderDate,
                DueDate = item.DueDate,
                ShippingRate = item.ShippingRate,
                PromotionCode = item.PromotionCode,
                SubTotal = item.SubTotal,
                TotalFees = item.TotalFees,
                Discount = item.Discount,
                Tax = item.Tax,
                CurrencyCode = item.CurrencyCode,
                Comment = item.Comment,
                MarkAsRead = item.MarkAsRead,
                Status = item.Status,
                CreatedDate = item.CreatedDate,
                LastModifiedDate = item.LastModifiedDate,
                VendorId = item.VendorId,
                TransactionMethodId = item.TransactionMethodId,
                CustomerId = item.CustomerId,
                Customer = customer,
                TransactionMethod = item.TransactionMethod.ToDto<TransactionMethod, TransactionMethodDetail>(),
                OrderPayment = item.OrderPayment.ToDto<OrderPayment, OrderPaymentDetail>(),
                Shipment = item.Shipment.ToDto<OrderShipment, OrderShipmentDetail>(),
                PaymentMethod = item.PaymentMethod.ToDto<PaymentMethod, PaymentMethodDetail>(),
                OrderProducts = GetOrderProductsByOrderNo(item.OrderNo),
            };
            return orderTemp;
        }

        public OrderTempDetail InsertOrderTemp(int vendorId, OrderEntry entry)
        {
            ISpecification<OrderEntry> validator = new OrderEntryValidator(UnitOfWork, PermissionLevel.Edit, CurrentClaimsIdentity);
            var dataViolations = new List<RuleViolation>();
            var isDataValid = validator.IsSatisfyBy(entry, dataViolations);
            if (!isDataValid) throw new ValidationError(dataViolations);

            var entity = entry.ToEntity<OrderEntry, OrderTemp>();

            entity.OrderNo = Guid.NewGuid();
            entity.VendorId = vendorId;
            entity.CreatedDate = DateTime.UtcNow;

            UnitOfWork.OrderTempRepository.Insert(entity);
            UnitOfWork.SaveChanges();

            return entity.ToDto<OrderTemp, OrderTempDetail>();
        }

        public void UpdateOrderTemp(int id, OrderEntry entry)
        {
            ISpecification<OrderEntry> validator = new OrderEntryValidator(UnitOfWork, PermissionLevel.Edit, CurrentClaimsIdentity);
            var dataViolations = new List<RuleViolation>();
            var isDataValid = validator.IsSatisfyBy(entry, dataViolations);
            if (!isDataValid) throw new ValidationError(dataViolations);

            var entity = UnitOfWork.OrderTempRepository.FindById(id);
            if (entity == null)
            {
                dataViolations.Add(new RuleViolation(ErrorCode.NotFoundOrder, "Order"));
                throw new ValidationError(dataViolations);
            }

            entity.CustomerId = entry.CustomerId;
            entity.ShippingRate = entry.ShippingRate;
            entity.PromotionCode = entry.PromotionCode;
            entity.SubTotal = entry.SubTotal;
            entity.TotalFees = entry.TotalFees;
            entity.Discount = entry.Discount;
            entity.Tax = entry.Tax;
            entity.CurrencyCode = entry.CurrencyCode;
            entity.Comment = entry.Comment;
            entity.LastModifiedDate = DateTime.UtcNow;

            UnitOfWork.OrderTempRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }

        public void UpdateOrderTemp(Guid orderNo, OrderEntry entry)
        {
            ISpecification<OrderEntry> validator = new OrderEntryValidator(UnitOfWork, PermissionLevel.Edit, CurrentClaimsIdentity);
            var dataViolations = new List<RuleViolation>();
            var isDataValid = validator.IsSatisfyBy(entry, dataViolations);
            if (!isDataValid) throw new ValidationError(dataViolations);

            var entity = UnitOfWork.OrderTempRepository.FindByOrderNo(orderNo);
            if (entity == null)
            {
                dataViolations.Add(new RuleViolation(ErrorCode.NotFoundOrder, "Order"));
                throw new ValidationError(dataViolations);
            }

            entity.CustomerId = entry.CustomerId;
            entity.ShippingRate = entry.ShippingRate;
            entity.PromotionCode = entry.PromotionCode;
            entity.SubTotal = entry.SubTotal;
            entity.TotalFees = entry.TotalFees;
            entity.Discount = entry.Discount;
            entity.Tax = entry.Tax;
            entity.CurrencyCode = entry.CurrencyCode;
            entity.Comment = entry.Comment;
            entity.LastModifiedDate = DateTime.UtcNow;

            UnitOfWork.OrderTempRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }

        /// <summary>
        /// Clear order temp and order product temp
        /// </summary>
        /// <param name="orderNo"></param>
        public void ClearOrderTemp(Guid orderNo)
        {
            RemoveOrderProductTemp(orderNo);

            var orderTempEntity = UnitOfWork.OrderTempRepository.FindByOrderNo(orderNo);
            if (orderTempEntity != null)
            {
                UnitOfWork.OrderTempRepository.DeleteByOrderNo(orderNo);
                UnitOfWork.SaveChanges();
            }
        }

        #endregion

        #region Order Product

        public IEnumerable<OrderProductDetail> GetOrderProducts(Guid orderNo)
        {
            var lst = UnitOfWork.OrderProductRepository.GetOrderProductsByOrderNo(orderNo);
            return lst.ToDtos<OrderProduct, OrderProductDetail>();
        }
        public IEnumerable<OrderProductInfoDetail> GetOrderProducts(int vendorId, ItemType type, DateTime? startDate, DateTime? endDate, OrderProductStatus? status)
        {
            var lst = new List<OrderProductInfoDetail>();
            var orderProducts =
                UnitOfWork.OrderProductRepository.GetOrderProducts(vendorId, type, startDate, endDate, status);
            if (orderProducts == null || orderProducts.Count() == 0) return null;

            foreach (var item in orderProducts)
            {
                var orderProduct = new OrderProductInfoDetail
                {
                    OrderProductId = item.OrderProductId,
                    OrderNo = item.OrderNo,
                    TypeId = item.TypeId,
                    ProductId = item.ProductId,
                    CustomerId = item.CustomerId,
                    EmployeeId = item.EmployeeId,
                    Quantity = item.Quantity,
                    Weight = item.Weight,
                    NetPrice = item.NetPrice,
                    GrossPrice = item.GrossPrice,
                    DiscountRate = item.DiscountRate,
                    TaxRate = item.TaxRate,
                    CurrencyCode = item.CurrencyCode,
                    PeriodGroupId = item.PeriodGroupId,
                    FromPeriod = item.FromPeriod,
                    ToPeriod = item.ToPeriod,
                    StartDate = item.StartDate,
                    EndDate = item.EndDate,
                    Comment = item.Comment,
                    Status = item.Status,
                    CreatedDate = item.CreatedDate,
                    LastModifiedDate = item.LastModifiedDate,
                    Item = GetOItemDetailsByProductId(item.ProductId, item.TypeId)
                };

                if (item.CustomerId != null && item.CustomerId > 0)
                {
                    orderProduct.Customer = CustomerService.GetCustomerInfoDetail(Convert.ToInt32(item.CustomerId));
                }

                if (item.EmployeeId != null && item.EmployeeId > 0)
                {
                    orderProduct.Employee = EmployeeService.GetEmployeeDetail(Convert.ToInt32(item.EmployeeId));
                }

                lst.Add(orderProduct);
            }

            return lst;
        }
        public IEnumerable<OrderProductInfoDetail> GetOrderProductsByOrderNo(Guid orderNo)
        {
            var lst = new List<OrderProductInfoDetail>();
            var orderProducts = UnitOfWork.OrderProductRepository.GetOrderProductsByOrderNo(orderNo).ToList();
            if (!orderProducts.Any()) return null;

            foreach (var item in orderProducts)
            {
                var orderProduct = new OrderProductInfoDetail
                {
                    OrderProductId = item.OrderProductId,
                    OrderNo = item.OrderNo,
                    TypeId = item.TypeId,
                    ProductId = item.ProductId,
                    CustomerId = item.CustomerId,
                    EmployeeId = item.EmployeeId,
                    Quantity = item.Quantity,
                    Weight = item.Weight,
                    NetPrice = item.NetPrice,
                    GrossPrice = item.GrossPrice,
                    DiscountRate = item.DiscountRate,
                    TaxRate = item.TaxRate,
                    CurrencyCode = item.CurrencyCode,
                    PeriodGroupId = item.PeriodGroupId,
                    FromPeriod = item.FromPeriod,
                    ToPeriod = item.ToPeriod,
                    StartDate = item.StartDate,
                    EndDate = item.EndDate,
                    Comment = item.Comment,
                    Status = item.Status,
                    CreatedDate = item.CreatedDate,
                    LastModifiedDate = item.LastModifiedDate,
                    Item = GetOItemDetailsByProductId(item.ProductId, item.TypeId)
                };

                if (item.CustomerId != null && item.CustomerId > 0)
                {
                    orderProduct.Customer = CustomerService.GetCustomerInfoDetail(Convert.ToInt32(item.CustomerId));
                }

                if (item.EmployeeId != null && item.EmployeeId > 0)
                {
                    orderProduct.Employee = EmployeeService.GetEmployeeDetail(Convert.ToInt32(item.EmployeeId));
                }

                lst.Add(orderProduct);
            }

            return lst;
        }
        public IEnumerable<OrderProductDetail> GetOrderProductsByCustomer(int vendorId, int customerId)
        {
            var lst = UnitOfWork.OrderProductRepository.GetOrderProductsByCustomer(vendorId, customerId);
            return lst.ToDtos<OrderProduct, OrderProductDetail>();
        }
        public OrderProductInfoDetail GetOrderProductDetails(int orderProductId)
        {
            var item = UnitOfWork.OrderProductRepository.FindById(orderProductId);
            if (item == null) return null;

            var orderProduct = new OrderProductInfoDetail
            {
                OrderProductId = item.OrderProductId,
                OrderNo = item.OrderNo,
                TypeId = item.TypeId,
                ProductId = item.ProductId,
                CustomerId = item.CustomerId,
                EmployeeId = item.EmployeeId,
                Quantity = item.Quantity,
                Weight = item.Weight,
                NetPrice = item.NetPrice,
                GrossPrice = item.GrossPrice,
                DiscountRate = item.DiscountRate,
                TaxRate = item.TaxRate,
                CurrencyCode = item.CurrencyCode,
                PeriodGroupId = item.PeriodGroupId,
                FromPeriod = item.FromPeriod,
                ToPeriod = item.ToPeriod,
                StartDate = item.StartDate,
                EndDate = item.EndDate,
                Comment = item.Comment,
                Status = item.Status,
                CreatedDate = item.CreatedDate,
                LastModifiedDate = item.LastModifiedDate,
                Item = GetOItemDetailsByProductId(item.ProductId, item.TypeId)
            };

            if (item.CustomerId != null && item.CustomerId > 0)
            {
                orderProduct.Customer = CustomerService.GetCustomerInfoDetail(Convert.ToInt32(item.CustomerId));
            }

            if (item.EmployeeId != null && item.EmployeeId > 0)
            {
                orderProduct.Employee = EmployeeService.GetEmployeeDetail(Convert.ToInt32(item.EmployeeId));
            }

            return orderProduct;
        }
        public OrderProductDetail GetOrderProductDetails(Guid orderNo, int productId)
        {
            var item = UnitOfWork.OrderProductRepository.GetDetails(orderNo, productId);
            return item.ToDto<OrderProduct, OrderProductDetail>();
        }
        public bool HasOrderProductExisted(int customerId, Guid orderNo, int productId)
        {
            return UnitOfWork.OrderProductRepository.HasOrderProductExisted(customerId, orderNo, productId);

        }
        public OrderProductDetail InsertOrderProduct(OrderProductEntry entry)
        {
            ISpecification<OrderProductEntry> validator = new OrderProductEntryValidator(UnitOfWork, PermissionLevel.Edit, CurrentClaimsIdentity);
            var dataViolations = new List<RuleViolation>();
            var isDataValid = validator.IsSatisfyBy(entry, dataViolations);
            if (!isDataValid) throw new ValidationError(dataViolations);

            var orderProduct = UnitOfWork.OrderProductRepository.HasOrderProductExisted(entry.OrderNo, entry.ProductId);
            if (orderProduct) return null;

            var entity = entry.ToEntity<OrderProductEntry, OrderProduct>();
            entity.Status = OrderProductStatus.Active;
            entity.CreatedDate = DateTime.UtcNow;

            UnitOfWork.OrderProductRepository.Insert(entity);
            UnitOfWork.SaveChanges();

            return entity.ToDto<OrderProduct, OrderProductDetail>();
        }
        public OrderProductDetail UpdateOrderProduct(OrderProductEditEntry entry)
        {
            ISpecification<OrderProductEntry> validator = new OrderProductEntryValidator(UnitOfWork, PermissionLevel.Edit, CurrentClaimsIdentity);
            var dataViolations = new List<RuleViolation>();
            var isDataValid = validator.IsSatisfyBy(entry, dataViolations);
            if (!isDataValid) throw new ValidationError(dataViolations);

            var entity = UnitOfWork.OrderProductRepository.GetDetails(entry.OrderNo, entry.ProductId);
            if (entity == null)
            {
                dataViolations.Add(new RuleViolation(ErrorCode.NotFoundOrder, "OrderProductEntry", entry.OrderNo));
                throw new ValidationError(dataViolations);
            }

            var from = entry.StartDate != null
                ? Convert.ToDateTime(entry.StartDate).Date
                : DateTime.Now.Date;
            var to = entry.EndDate != null
                ? Convert.ToDateTime(entry.EndDate).Date
                : from;
            var fromPeriod = entry.FromPeriod != null ? Convert.ToInt32(entry.FromPeriod) : 0;
            var toPeriod = entry.ToPeriod != null ? Convert.ToInt32(entry.ToPeriod) : 0;
            var start = from.AddMinutes(fromPeriod);
            var end = to.AddMinutes(toPeriod);

            entity.OrderNo = entry.OrderNo;
            entity.TypeId = entry.TypeId;
            entity.ProductId = entry.ProductId;
            entity.CustomerId = entry.CustomerId;
            entity.EmployeeId = entry.EmployeeId;
            entity.Quantity = entry.Quantity;
            entity.Weight = entry.Weight;
            entity.NetPrice = entry.NetPrice;
            entity.GrossPrice = entry.GrossPrice;
            entity.DiscountRate = entry.DiscountRate;
            entity.TaxRate = entry.TaxRate;
            entity.CurrencyCode = entry.CurrencyCode;
            entity.PeriodGroupId = entry.PeriodGroupId;
            entity.FromPeriod = fromPeriod;
            entity.ToPeriod = toPeriod;
            entity.StartDate = start;
            entity.EndDate = end;
            entity.Comment = entry.Comment;
            entity.LastModifiedDate = DateTime.UtcNow;

            UnitOfWork.OrderProductRepository.Update(entity);
            UnitOfWork.SaveChanges();

            return entity.ToDto<OrderProduct, OrderProductDetail>();
        }
        public void RemoveOrderProduct(Guid orderNo)
        {
            var lst = UnitOfWork.OrderProductRepository.GetOrderProductsByOrderNo(orderNo).ToList();
            if (lst.Any())
            {
                foreach (var item in lst)
                {
                    RemoveOrderProduct(item.OrderNo, item.ProductId);
                }
            }
        }
        public void RemoveOrderProduct(Guid orderNo, int productId)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.OrderProductRepository.GetDetails(orderNo, productId);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundOrderProduct, "OrderProduct", orderNo, ErrorMessage.Messages[ErrorCode.NotFoundOrderProduct]));
                throw new ValidationError(violations);
            }

            UnitOfWork.OrderProductRepository.Delete(entity);
            UnitOfWork.SaveChanges();
        }

        public ItemDetail GetOItemDetailsByProductId(int productId, ItemType typeId)
        {
            if (typeId == ItemType.Product)
            {
                var objProduct = UnitOfWork.ProductRepository.GetDetailsByProductId(productId);
                if (objProduct != null)
                    return new ItemDetail
                    {
                        ItemId = objProduct.Id,
                        ItemCode = objProduct.ProductCode,
                        ItemName = objProduct.ProductName,
                        ItemAlias = objProduct.ProductAlias,
                        GrossPrice = objProduct.GrossPrice,
                        CurrencyCode = objProduct.CurrencyCode
                    };
            }

            if (typeId == ItemType.ServicePackage)
            {
                var objService = UnitOfWork.ServicePackRepository.GetDetail(productId);
                if (objService != null)
                    return new ItemDetail
                    {
                        ItemId = objService.PackageId,
                        ItemCode = objService.PackageCode,
                        ItemName = objService.PackageName,
                        ItemAlias = objService.PackageName,
                        GrossPrice = objService.PackageFee,
                        CurrencyCode = objService.CurrencyCode
                    };
            }
            return null;
        }
        #endregion

        #region Order Product Temp

        public OrderProductDetail InsertOrderProductTemp(OrderProductTempEntry entry)
        {
            ISpecification<OrderProductTempEntry> validator = new OrderProductTempEntryValidator(UnitOfWork, PermissionLevel.Edit, CurrentClaimsIdentity);
            var dataViolations = new List<RuleViolation>();
            var isDataValid = validator.IsSatisfyBy(entry, dataViolations);
            if (!isDataValid) throw new ValidationError(dataViolations);

            var orderProduct = UnitOfWork.OrderProductTempRepository.HasOrderProductExisted(entry.OrderNo, entry.ProductId);
            if (orderProduct) return null;

            var entity = entry.ToEntity<OrderProductTempEntry, OrderProductTemp>();
            entity.Status = OrderProductTempStatus.Active;
            entity.CreatedDate = DateTime.UtcNow;

            UnitOfWork.OrderProductTempRepository.Insert(entity);
            UnitOfWork.SaveChanges();

            return entity.ToDto<OrderProductTemp, OrderProductDetail>();
        }
        public OrderProductDetail UpdateOrderProductTemp(OrderProductTempEditEntry entry)
        {
            ISpecification<OrderProductTempEditEntry> validator = new OrderProductTempEditEntryValidator(UnitOfWork, PermissionLevel.Edit, CurrentClaimsIdentity);
            var dataViolations = new List<RuleViolation>();
            var isDataValid = validator.IsSatisfyBy(entry, dataViolations);
            if (!isDataValid) throw new ValidationError(dataViolations);

            var entity = UnitOfWork.OrderProductTempRepository.GetDetails(entry.OrderNo, entry.ProductId);
            if (entity == null)
            {
                dataViolations.Add(new RuleViolation(ErrorCode.NotFoundOrder, "Order", entry.OrderNo));
                throw new ValidationError(dataViolations);
            }

            entity.OrderNo = entry.OrderNo;
            entity.TypeId = entry.TypeId;
            entity.ProductId = entry.ProductId;
            entity.CustomerId = entry.CustomerId;
            entity.EmployeeId = entry.EmployeeId;
            entity.Quantity = entry.Quantity;
            entity.Weight = entry.Weight;
            entity.NetPrice = entry.NetPrice;
            entity.GrossPrice = entry.GrossPrice;
            entity.DiscountRate = entry.DiscountRate;
            entity.TaxRate = entry.TaxRate;
            entity.CurrencyCode = entry.CurrencyCode;
            entity.LastModifiedDate = DateTime.UtcNow;

            UnitOfWork.OrderProductTempRepository.Update(entity);
            UnitOfWork.SaveChanges();

            return entity.ToDto<OrderProductTemp, OrderProductDetail>();
        }

        public void RemoveOrderProductTemp(Guid orderNo)
        {
            var lst = UnitOfWork.OrderProductTempRepository.GetListByOrderNo(orderNo).ToList();
            if (lst.Any())
            {
                foreach (var item in lst)
                {
                    RemoveOrderProductTemp(item.OrderNo, item.ProductId);
                }
            }
        }
        public void RemoveOrderProductTemp(Guid orderNo, int productId)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.OrderProductTempRepository.GetDetails(orderNo, productId);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundOrderProduct, "OrderProduct", orderNo, ErrorMessage.Messages[ErrorCode.NotFoundOrderProduct]));
                throw new ValidationError(violations);
            }

            UnitOfWork.OrderProductTempRepository.Delete(entity);
            UnitOfWork.SaveChanges();
        }
        #endregion

        #region Order Shipment
        public OrderShipmentDetail InsertOrderShipment(OrderShipmentEntry entry)
        {
            ISpecification<OrderShipmentEntry> validator = new OrderShipmentEntryValidator(UnitOfWork, PermissionLevel.Edit, CurrentClaimsIdentity);
            var dataViolations = new List<RuleViolation>();
            var isDataValid = validator.IsSatisfyBy(entry, dataViolations);
            if (!isDataValid) throw new ValidationError(dataViolations);

            var entity = entry.ToEntity<OrderShipmentEntry, OrderShipment>();
            entity.CreatedDate = DateTime.UtcNow;

            UnitOfWork.OrderShipmentRepository.Insert(entity);
            UnitOfWork.SaveChanges();

            return entity.ToDto<OrderShipment, OrderShipmentDetail>();
        }

        public OrderShipmentDetail UpdateOrderShipment(OrderShipmentEditEntry entry)
        {
            ISpecification<OrderShipmentEntry> validator = new OrderShipmentEntryValidator(UnitOfWork, PermissionLevel.Edit, CurrentClaimsIdentity);
            var dataViolations = new List<RuleViolation>();
            var isDataValid = validator.IsSatisfyBy(entry, dataViolations);
            if (!isDataValid) throw new ValidationError(dataViolations);

            var entity = UnitOfWork.OrderShipmentRepository.GetDetails(entry.OrderNo, entry.ShippingMethodId);
            if (entity == null)
            {
                dataViolations.Add(new RuleViolation(ErrorCode.NotFoundOrder, "Order", entry.OrderNo));
                throw new ValidationError(dataViolations);
            }

            entity.OrderNo = entry.OrderNo;
            entity.CustomerId = entry.CustomerId;
            entity.ShippingMethodId = entry.ShippingMethodId;
            entity.ShipDate = entry.ShipDate;
            entity.Weight = entry.Weight;
            entity.IsInternational = entry.IsInternational;
            entity.ReceiverName = entry.ReceiverName;
            entity.ReceiverEmail = entry.ReceiverEmail;
            entity.ReceiverPhone = entry.ReceiverPhone;
            entity.ReceiverAddress = entry.ReceiverAddress;
            entity.CountryId = entry.CountryId;
            entity.ProvinceId = entry.ProvinceId;
            entity.CityId = entry.CityId;
            entity.RegionId = entry.RegionId;
            entity.PostalCode = entry.PostalCode;
            entity.LastModifiedDate = DateTime.UtcNow;

            UnitOfWork.OrderShipmentRepository.Update(entity);
            UnitOfWork.SaveChanges();

            return entity.ToDto<OrderShipment, OrderShipmentDetail>();
        }
        #endregion

        #region Order Payment
        public OrderPaymentDetail InsertOrderPayment(OrderPaymentEntry entry)
        {
            //ISpecification<OrderPaymentEntry> validator = new OrderPaymentEntryValidator(UnitOfWork, PermissionLevel.Edit, CurrentClaimsIdentity);
            //var dataViolations = new List<RuleViolation>();
            //var isDataValid = validator.IsSatisfyBy(entry, dataViolations);
            //if (!isDataValid) throw new ValidationError(dataViolations);

            var entity = entry.ToEntity<OrderPaymentEntry, OrderPayment>();

            UnitOfWork.OrderPaymentRepository.Insert(entity);
            UnitOfWork.SaveChanges();

            return entity.ToDto<OrderPayment, OrderPaymentDetail>();
        }

        public OrderPaymentDetail UpdateOrderPayment(OrderPaymentEditEntry entry)
        {
            //ISpecification<OrderPaymentEditEntry> validator = new OrderPaymentEntryValidator(UnitOfWork, PermissionLevel.Edit, CurrentClaimsIdentity);
            var dataViolations = new List<RuleViolation>();
            //var isDataValid = validator.IsSatisfyBy(entry, dataViolations);
            //if (!isDataValid) throw new ValidationError(dataViolations);

            var entity = UnitOfWork.OrderPaymentRepository.GetDetails(entry.OrderNo, entry.PaymentMethodId);
            if (entity == null)
            {
                dataViolations.Add(new RuleViolation(ErrorCode.NotFoundOrder, "Order", entry.OrderNo));
                throw new ValidationError(dataViolations);
            }

            entity.OrderNo = entry.OrderNo;
            entity.Amount = entry.Amount;
            entity.CustomerId = entry.CustomerId;
            entity.PaymentMethodId = entry.PaymentMethodId;
            entity.PaymentCode = entry.PaymentCode;
            entity.CardType = entry.CardType;
            entity.CardHolder = entry.CardHolder;
            entity.CardNo = entry.CardNo;
            entity.ExpMonth = entry.ExpMonth;
            entity.ExpYear = entry.ExpYear;
            entity.Cvv = entry.Cvv;

            UnitOfWork.OrderPaymentRepository.Update(entity);
            UnitOfWork.SaveChanges();

            return entity.ToDto<OrderPayment, OrderPaymentDetail>();
        }

        #endregion

        #region Order Promotion
        public PromotionDetail GetPromotion(int vendorId, string promotionCode, DateTime date)
        {
            var promotion = UnitOfWork.PromotionRepository.Get().FirstOrDefault(x => x.VendorId == vendorId
                                        && x.PromotionCode.ToLower() == promotionCode.ToLower()
                                        && (x.StartDate.HasValue == false || (x.StartDate.HasValue && x.StartDate.Value <= date))
                                        && (x.EndDate.HasValue == false || (x.EndDate.HasValue && x.EndDate.Value >= date))
                                        && x.IsActive == PromotionStatus.Active);
            return promotion.ToDto<Promotion, PromotionDetail>();
        }
        public void ApplyPromotion(int vendorId, string promotionCode, ShoppingCart cart)
        {
            var promotion = GetPromotion(vendorId, promotionCode, DateTime.UtcNow);
            if (promotion != null)
            {
                cart.PromotionInfo = new PromotionInfo
                {
                    PromotionId = promotion.PromotionId,
                    PromotionCode = promotion.PromotionCode,
                    PromotionTitle = promotion.PromotionTitle,
                    PromotionValue = promotion.PromotionValue,
                    IsPercent = promotion.IsPercent
                };
            }
        }
        public void RemovePromotion(ShoppingCart cart)
        {
            cart.PromotionInfo = null;
        }
        #endregion

        #region Order Purchase 
        public List<RuleViolation> ValidateTransaction(OrderTransactionEntry orderEntry)
        {
            var violations = new List<RuleViolation>();
            if (orderEntry == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NullOrderTransactionEntry, "OrderTransactionEntry", null, ErrorMessage.Messages[ErrorCode.NullOrderTransactionEntry]));
                throw new ValidationError(violations);
            }

            if (orderEntry.OrderProducts == null || !orderEntry.OrderProducts.Any())
            {
                violations.Add(new RuleViolation(ErrorCode.EmptyCart, "OrderProducts", null, ErrorMessage.Messages[ErrorCode.EmptyCart]));
                throw new ValidationError(violations);
            }

            //if (!ValidateUnitsInStock(orderEntry.OrderProducts.ToList()))
            //{
            //    violations.Add(new RuleViolation(ErrorCode.QuantityNotSufficient, "Quantity", null, ErrorMessage.Messages[ErrorCode.QuantityNotSufficient]));
            //    throw new ValidationError(violations);
            //}

            if (!ValidatePromotionCodeAvailable(orderEntry.PromotionCode))
            {
                violations.Add(new RuleViolation(ErrorCode.PromotionCodeUtilized, "PromotionCode", null, ErrorMessage.Messages[ErrorCode.PromotionCodeUtilized]));
                throw new ValidationError(violations);
            }
            return violations;
        }

        public bool ValidateUnitsInStock(List<OrderProductInfoDetail> orderProducts)
        {
            if (!orderProducts.Any() || orderProducts.Count == 0) return false;

            bool result = true;

            //Check products in the order detail with Product type
            var productsInOrders = orderProducts.Where(x => x.TypeId == ItemType.Product).ToList();
            if (productsInOrders.Any() && productsInOrders.Count > 0)
            {
                result = ValidateProductsInOrderDetails(productsInOrders);
            }

            //Check packages in the order detail with Service Package type
            var packagesInOrders = orderProducts.Where(x => x.TypeId == ItemType.ServicePackage).ToList();
            if (packagesInOrders.Any() && packagesInOrders.Count > 0)
            {
                result = ValidatePackagesInOrderDetails(packagesInOrders);
            }
            return result;
        }

        public bool ValidateProductsInOrderDetails(List<OrderProductInfoDetail> productsInOrders)
        {
            if (!productsInOrders.Any() || productsInOrders.Count == 0) return false;

            //Get product id in order product
            var productIds = productsInOrders.Where(x => x.TypeId == ItemType.Product).Select(p => p.ProductId).ToList();

            // Check whether products in order products with product type has been existed in the product table in database
            var products = UnitOfWork.ProductRepository.Get().Where(p => productIds.Contains(p.ProductId)).ToList();

            //Products in order is different from products in product table
            if (products.Count != productsInOrders.Count) return false;

            foreach (var product in products)
            {
                //Unallow to book product that is out of stock
                if (product.UnitsInStock == null || product.UnitsInStock == 0) return false;

                //Get product details of each item in order product with product type
                var productItemInOrder = productsInOrders.FirstOrDefault(p => p.ProductId == product.ProductId);
                if (productItemInOrder == null) return false;

                // Comparing product quantity in order and in stock
                if (productItemInOrder.Quantity > product.UnitsInStock.Value)
                {
                    return false;
                }
            }

            return true;
        }

        public bool ValidatePackagesInOrderDetails(List<OrderProductInfoDetail> packagesInOrders)
        {
            //Get package id in order product
            var packageIds = packagesInOrders.Where(x => x.TypeId == ItemType.ServicePackage).Select(p => p.ProductId).ToList();

            // Check whether packages in order products with service package type has been existed in the service package table in database
            var packages = UnitOfWork.ServicePackRepository.Get().Where(p => packageIds.Contains(p.PackageId)).ToList();

            //Products in order is different from products in product table
            if (packages.Count != packagesInOrders.Count) return false;

            foreach (var package in packages)
            {
                //Unallow to book product that is out of stock
                if (package.AvailableQuantity == null || package.AvailableQuantity == 0) return false;

                //Get product details of each item in order product with product type
                var packageItemInOrder = packagesInOrders.FirstOrDefault(p => p.ProductId == package.PackageId);
                if (packageItemInOrder == null) return false;

                // Comparing product quantity in order and in stock
                if (packageItemInOrder.Quantity > package.AvailableQuantity.Value)
                {
                    return false;
                }
            }

            return true;
        }

        public bool ValidatePromotionCodeAvailable(string promotionCode)
        {
            var result = true;
            if (!string.IsNullOrWhiteSpace(promotionCode))
            {
                var promotionCodeEntity = UnitOfWork.PromotionRepository.GetDetails(1, promotionCode);
                if (promotionCodeEntity != null && promotionCodeEntity.IsActive == PromotionStatus.InActive)
                {
                    result = false;
                }
                else
                {
                    result = false;
                }
            }
            return result;
        }

        public TransactionState PurchaseOrder(Guid applicationId, int vendorId, OrderTransactionEntry orderEntry)
        {
            var transactionState = new TransactionState();
            var violations = ValidateTransaction(orderEntry);
            if (violations != null && violations.Any())
            {
                transactionState.Errors = violations;
                return transactionState;
            }

            try
            {
                UnitOfWork.BeginTransaction();

                // 1. Save into Sales.Order
                var orderEntity = orderEntry.ToEntity<OrderTransactionEntry, Entities.Business.Orders.Order>();
                UnitOfWork.OrderRepository.Insert(orderEntity);

                // 2. Save into Sales.OrderPayment
                var orderPaymentEntity = new OrderPayment
                {
                    OrderNo = orderEntity.OrderNo,
                    PaymentMethodId = orderEntry.Payment.PaymentMethodId,
                    CardType = orderEntry.Payment.CardType,
                    CardHolder = orderEntry.Payment.CardHolder,
                    CardNo = orderEntry.Payment.CardNo,
                    ExpMonth = orderEntry.Payment.ExpMonth,
                    ExpYear = orderEntry.Payment.ExpYear,
                    Cvv = orderEntry.Payment.Cvv,
                    CustomerId= orderEntity.CustomerId
                };
                UnitOfWork.OrderPaymentRepository.Insert(orderPaymentEntity);

                // 3. Save into Sales.OrderProduct
                foreach (var orderProductEntry in orderEntry.OrderProducts)
                {
                    // var orderProduct
                    var orderProductEntity = orderProductEntry.ToEntity<OrderProductInfoDetail, OrderProduct>();
                    UnitOfWork.OrderProductRepository.Insert(orderProductEntity);

                    // 2.1. Subtract quantity
                    if (orderProductEntity.TypeId == ItemType.Product)
                    {
                        var productEntity = UnitOfWork.ProductRepository.FindById(orderProductEntry.ProductId);
                        if (productEntity != null)
                        {
                            productEntity.UnitsInStock -= orderProductEntry.Quantity;
                            productEntity.UnitsOnOrder += orderProductEntry.Quantity;
                            UnitOfWork.ProductRepository.Update(productEntity);
                        }
                    }

                    if (orderProductEntity.TypeId == ItemType.ServicePackage)
                    {
                        var packageEntity = UnitOfWork.ServicePackRepository.FindById(orderProductEntry.ProductId);
                        if (packageEntity != null)
                        {
                            packageEntity.AvailableQuantity -= orderProductEntry.Quantity;
                            //packageEntity.UnitsOnOrder += orderProductEntry.Quantity;
                            UnitOfWork.ServicePackRepository.Update(packageEntity);
                        }
                    }
                }

                // 3. Save into Sales.OrderShipment
                var orderShipmentEntity = orderEntry.Shipment.ToEntity<OrderShipmentEntry, OrderShipment>();
                orderShipmentEntity.OrderNo = orderEntry.OrderNo;
                orderShipmentEntity.CustomerId = orderEntry.CustomerId;
                orderShipmentEntity.CreatedDate = DateTime.UtcNow;
                UnitOfWork.OrderShipmentRepository.Insert(orderShipmentEntity);

                // 4. Disable Promotion (if any)
                if (!string.IsNullOrEmpty(orderEntry.PromotionCode))
                {
                    var promotionEntity = UnitOfWork.PromotionRepository.GetDetails(vendorId, orderEntry.PromotionCode);
                    if (promotionEntity != null && promotionEntity.IsActive == PromotionStatus.Active)
                    {
                        promotionEntity.IsActive = PromotionStatus.InActive;
                        promotionEntity.LastModifiedDate = DateTime.UtcNow;
                        UnitOfWork.PromotionRepository.Update(promotionEntity);
                    }
                }

                UnitOfWork.SaveChanges();
                UnitOfWork.Commit();

                transactionState.Order = orderEntity.ToDto<Entities.Business.Orders.Order, OrderDetail>();
                return transactionState;
            }
            catch (ValidationError ex)
            {
                UnitOfWork.Rollback();
                violations.Add(new RuleViolation(ErrorCode.CannotSaveOrder, "Order", ex.ToString(), ErrorMessage.Messages[ErrorCode.CannotSaveOrder]));
                transactionState.Errors = violations;
                return transactionState;
            }
        }

        public void UpdateOrderStatus(OrderTransactionEntry order)
        {
            var violations = ValidateTransaction(order);
            if (violations != null && violations.Any()) return;

            try
            {
                UnitOfWork.BeginTransaction();

                // 1. Save into Sales.Order
                var orderInfoEntity = order.ToEntity<OrderTransactionEntry, Entities.Business.Orders.Order>();
                //orderInfoEntity.State = Entities.ObjectState.Modified;
                UnitOfWork.OrderRepository.Update(orderInfoEntity);

                // 2. Save into Sales.OrderProduct
                foreach (var orderProductEntry in order.OrderProducts)
                {
                    UnitOfWork.OrderProductRepository.Insert(orderProductEntry.ToEntity<OrderProductInfoDetail, OrderProduct>());

                    // 2.1. Subtract quantity
                    var productEntity = UnitOfWork.ProductRepository.FindById(orderProductEntry.ProductId);
                    productEntity.UnitsInStock += orderProductEntry?.Quantity;
                    productEntity.UnitsOnOrder -= orderProductEntry?.Quantity;
                    UnitOfWork.ProductRepository.Update(productEntity);
                }

                UnitOfWork.SaveChanges();
                UnitOfWork.Commit();
            }
            catch (Exception ex)
            {
                UnitOfWork.Rollback();
                if (ex.InnerException != null) throw ex.InnerException;
            }
        }
        public bool ProcessPayment(Guid applicationId, OrderPaymentEntry payment, out string statePayment)
        {
            bool result;
            var violations = new List<RuleViolation>();
            try
            {
                if (payment == null)
                {
                    violations.Add(new RuleViolation(ErrorCode.NotFoundCustomerPayment, "OrderPaymentEntry", null,
                        ErrorMessage.Messages[ErrorCode.NotFoundCustomerPayment]));
                    throw new ValidationError(violations);
                }
                
                var order = GetOrderDetailByOrderNo(payment.OrderNo);
                if (order == null)
                {
                    violations.Add(new RuleViolation(ErrorCode.NotFoundOrder, "Order", null,
                        ErrorMessage.Messages[ErrorCode.NotFoundOrder]));
                    throw new ValidationError(violations);
                }
                
                if (order.Shipment == null)
                {
                    violations.Add(new RuleViolation(ErrorCode.NotFoundShipment, "Shipment", null,
                        ErrorMessage.Messages[ErrorCode.NotFoundShipment]));
                    throw new ValidationError(violations);
                }

                //Check whether country exists or not
                var country = UnitOfWork.CountryRepository.FindById(order.Shipment.CountryId);
                if (country == null)
                {
                    violations.Add(new RuleViolation(ErrorCode.NotFoundCountry, "CountryId", order.Shipment.CountryId,
                        ErrorMessage.Messages[ErrorCode.NotFoundCountry]));
                    throw new ValidationError(violations);
                }

                //Check whether province exists or not
                string provinceName = string.Empty;
                if (order.Shipment.ProvinceId != null && order.Shipment.ProvinceId > 0)
                {
                    var province = UnitOfWork.ProvinceRepository.FindById(order.Shipment.ProvinceId);
                    if (province == null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.NotFoundProvince, "ProvinceId", order.Shipment.ProvinceId,
                            ErrorMessage.Messages[ErrorCode.NotFoundProvince]));
                        throw new ValidationError(violations);
                    }
                    provinceName = province.ProvinceName;
                }

                //Check whether city exists or not
                string regionName;
                if (order.Shipment.RegionId != null && order.Shipment.RegionId > 0)
                {
                    var region = UnitOfWork.RegionRepository.FindById(order.Shipment.RegionId);
                    if (region == null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.NotFoundRegion, "RegionId", order.Shipment.RegionId,
                            ErrorMessage.Messages[ErrorCode.NotFoundRegion]));
                        throw new ValidationError(violations);
                    }
                    regionName = region.RegionName;
                }
                else
                {
                    if (string.IsNullOrEmpty(order.Shipment.ReceiverAddress))
                    {
                        violations.Add(new RuleViolation(ErrorCode.InvalidAddress, "ReceiverAddress",
                            order.Shipment.ReceiverAddress, ErrorMessage.Messages[ErrorCode.InvalidAddress]));
                        throw new ValidationError(violations);
                    }
                    regionName = order.Shipment.ReceiverAddress;
                }

                //Check whether post code exists or not
                if (string.IsNullOrEmpty(order.Shipment.PostalCode))
                {
                    violations.Add(new RuleViolation(ErrorCode.NotFoundPostalCode, "PostalCode", order.Shipment.PostalCode,
                        ErrorMessage.Messages[ErrorCode.NotFoundPostalCode]));
                    throw new ValidationError(violations);
                }

                var billingAddress = new BillingAddressDetail //Address for the payment
                {
                    CountryCode = country.Iso,
                    City = regionName,
                    State = provinceName,
                    Line1 = order.Shipment.ReceiverAddress,
                    PostalCode = order.Shipment.PostalCode,
                    Phone = order.Shipment.ReceiverPhone
                };

                //RedirectUrls - http://localhost:3000/cancel or http://localhost:3000/process or paypalObject.SiteURL + "?cancel=true" 
                var baseUrl = HttpContext.Current.Request.Url.Authority;
                if (baseUrl.EndsWith("/"))
                {
                    baseUrl = baseUrl.Substring(0, baseUrl.Length - 1);
                }

                var redirectUrls = new TransactionRedirectUrls
                {
                    CancelUrl = $"{baseUrl}/CustOrder/TransactionFailure",
                    ReturnUrl = $"{baseUrl}/CustOrder/TransactionSuccess"
                };

                //Check whether customer exists or not
                var customer = CustomerService.GetCustomerInfoDetail(order.CustomerId);
                if (customer == null)
                {
                    violations.Add(new RuleViolation(ErrorCode.NotFoundCustomer, "Customer", order.CustomerId,
                        ErrorMessage.Messages[ErrorCode.NotFoundCustomer]));
                    throw new ValidationError(violations);
                }
                             
                var transactionMethod = UnitOfWork.TransactionMethodRepository.FindById(order.TransactionMethodId);
                if (transactionMethod == null)
                {
                    violations.Add(new RuleViolation(ErrorCode.NotFoundTransactionMethod, "TransactionMethodId", order.TransactionMethodId,
                        ErrorMessage.Messages[ErrorCode.NotFoundTransactionMethod]));
                    throw new ValidationError(violations);
                }

                var paymentMethod = UnitOfWork.PaymentMethodRepository.FindById(payment.PaymentMethodId);
                if (paymentMethod == null)
                {
                    violations.Add(new RuleViolation(ErrorCode.NotFoundPaymentMethod, "PaymentMethodId", payment.PaymentMethodId,
                        ErrorMessage.Messages[ErrorCode.NotFoundPaymentMethod]));
                    throw new ValidationError(violations);
                }

                // 2. Save into Sales.OrderPayment
                var orderPaymentEntry = new OrderPaymentEntry
                {
                    OrderNo = order.OrderNo,
                    Amount = order.TotalFees,
                    CustomerId = order.CustomerId,
                    PaymentToken = payment.PaymentToken,
                    PaymentMethodId = payment.PaymentMethodId,
                    CardType = payment.CardType,
                    CardHolder = payment.CardHolder,
                    CardNo = payment.CardNo.Replace(" ", string.Empty),
                    ExpMonth = payment.ExpMonth,
                    ExpYear = payment.ExpYear,
                    Cvv = payment.Cvv
                };
                var orderPayment =InsertOrderPayment(orderPaymentEntry);
                if (orderPayment == null)
                {
                    violations.Add(new RuleViolation(ErrorCode.NotFoundOrderPayment, "OrderPayment", null,
                        ErrorMessage.Messages[ErrorCode.NotFoundOrderPayment]));
                    throw new ValidationError(violations);
                }
                order.OrderPayment = orderPayment;

                var paymentMethodSetting = (PaymentMethodSetting)payment.PaymentMethodId;
                switch (paymentMethodSetting)
                {
                    case PaymentMethodSetting.Stripe:
                        result = PaymentWithStripeCreditCard(applicationId, customer, billingAddress, redirectUrls, order, out statePayment);
                        break;
                    case PaymentMethodSetting.Paypal:
                        if (paymentMethod.IsCreditCard || order.TransactionMethodId == 1)
                        {
                            result = PaymentWithPayPalCreditCard(customer, billingAddress, redirectUrls, order, out statePayment);
                        }
                        else
                        {
                            string payerId = string.Empty;
                            result = PaymentWithPayPal(applicationId, order, payerId, out statePayment);
                        }
                        break;
                    case PaymentMethodSetting.Offline:
                        statePayment = PaymentState.Created;
                        result = true;
                        break;
                    default:
                        statePayment = PaymentState.Created;
                        result = true;
                        break;
                }

                if (result)
                {
                    //Send Notification
                    SendOrderCompleteNotification(applicationId, order, ShoppingCart.Instance, customer.CustomerId.ToString(), DateTime.UtcNow,
                        VendorService.GetDefaultVendor().Email, string.Empty);
                }

                return result;
            }
            catch (Exception ex)
            {
                statePayment = ex.ToString();
                result = false;
                return result;
            }
        }
        
        private void SendOrderCompleteNotification(Guid applicationId, OrderInfoDetail order, ShoppingCart cart, string targetId, DateTime predefinedDate, string cc, string bcc)
        {
            try
            {
                var violations = new List<RuleViolation>();
                var notificationTypeId = Convert.ToInt32(NotificationTypeSetting.OrderComplete);
                var messageTypeSettingId = Convert.ToInt32(MessageTypeSetting.Email);
                var notificationType = UnitOfWork.NotificationTypeRepository.FindById(notificationTypeId);
                if (notificationType == null) return;

                //Get Template 
                var template = MessageService.GetMessageTemplateDetail(notificationTypeId, messageTypeSettingId);
                if (template == null)
                {
                    violations.Add(new RuleViolation(ErrorCode.NotFoundForMessageTemplate, "MessageTemplate", null, ErrorMessage.Messages[ErrorCode.NotFoundForMessageTemplate]));
                    throw new ValidationError(violations);
                }

                //Get Mail Settings
                var mailSettings = MailService.GetDefaultSmtpInfo(applicationId);
                if (mailSettings == null)
                {
                    violations.Add(new RuleViolation(ErrorCode.NotFoundNotificationSender, "NotificationSender", null, ErrorMessage.Messages[ErrorCode.NotFoundNotificationSender]));
                    throw new ValidationError(violations);
                }

                // Get Sender Information
                var sender = NotificationService.GetNotificationSenderDetail(notificationType.NotificationSenderTypeId);

                //Get Receivers
                var receivers = NotificationService.GetNotificationTargets(notificationTypeId, targetId);
                if (receivers.Any())
                {
                    //Check whether customer exists or not
                    var customer = UnitOfWork.CustomerRepository.FindById(order.CustomerId);
                    if (customer == null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.NotFoundCustomer, "Customer", null, ErrorMessage.Messages[ErrorCode.NotFoundCustomer]));
                        throw new ValidationError(violations);
                    }

                    string customerName = $"{customer.FirstName} {customer.LastName}";
                    string customerEmail = customer.Email;

                    var vendor = VendorService.GetDefaultVendor();

                    var subjectVariables = new Hashtable{
                        {"VendorName", vendor.VendorName },
                        {"OrderNo", order.OrderNo }
                    };

                    string subject = ParseTemplateHandler.ParseTemplate(subjectVariables, template.TemplateSubject);

                    foreach (var receiver in receivers)
                    {
                        var templateVariables = new Hashtable
                        {
                            {"CustomerName", customerName },
                            {"OrderNo", order.OrderNo },
                            {"OrderDate", order.OrderDate },
                            {"OrderDetails", ConvertOrderDetailsToMailTemplete(order, cart) }
                        };

                        string body = ParseTemplateHandler.ParseTemplate(templateVariables, template.TemplateBody);

                        //Create message queue
                        var messageQueueEntry = new MessageQueueEntry
                        {
                            From = sender.MailAddress,
                            To = customerEmail,
                            Subject = subject,
                            Body = body,
                            Bcc = bcc,
                            Cc = cc,
                            PredefinedDate = predefinedDate
                        };

                        var messageQueue = MessageService.CreateMessageQueue(messageQueueEntry);

                        //Create notification message
                        var extraInfo = (from string key in templateVariables.Keys
                                         select new SerializableKeyValuePair<string, string>
                                         {
                                             Key = key,
                                             Value = Convert.ToString(templateVariables[key])
                                         }).ToList();

                        var messsageInfo = new MessageDetail
                        {
                            MessageTypeId = MessageTypeSetting.Email,
                            NotificationTypeId = NotificationTypeSetting.OrderComplete,
                            TemplateId = template.TemplateId,
                            WebsiteUrl = string.Empty,
                            WebsiteUrlBase = string.Empty,
                            ExtraInfo = extraInfo,
                            Version = "1.0"
                        };

                        var notificationMessageEntry = new NotificationMessageEntry
                        {
                            Message = messsageInfo,
                            PublishDate = predefinedDate,
                            SentStatus = NotificationSentStatus.Ready
                        };
                        var notificationMessage = NotificationService.InsertNotificationMessage(notificationMessageEntry);

                        //Send Mail Manually
                        var isEmailSent = MailHandler.SendMail(mailSettings.SmtpmEmail, customerEmail, sender.SenderName, receiver.TargetName, cc, bcc, null, MailPriority.Normal, subject, true, Encoding.UTF8, body, null,
                            mailSettings.SmtpServer, SmtpAuthentication.Basic, mailSettings.SmtpUsername, mailSettings.SmtpPassword, mailSettings.EnableSsl, out var result);

                        //Update status in message queue
                        var messageQueueEditEntry = new MessageQueueEditEntry
                        {
                            QueueId = messageQueue.QueueId,
                            From = messageQueue.From,
                            To = messageQueue.To,
                            Subject = messageQueue.Subject,
                            Body = messageQueue.Body,
                            Bcc = messageQueue.Bcc,
                            Cc = messageQueue.Cc,
                            Status = isEmailSent,
                            ResponseStatus = isEmailSent ? 1 : 0,
                            ResponseMessage = result,
                            SentDate = DateTime.UtcNow
                        };
                        MessageService.UpdateMessageQueue(messageQueueEditEntry);


                        //Update status in notification message
                        var sentStatus = isEmailSent ? NotificationSentStatus.Sent : NotificationSentStatus.Failed;
                        NotificationService.UpdateNotificationMessageStatus(notificationMessage.NotificationMessageId,
                            messsageInfo, sentStatus);
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null) throw ex.InnerException;
            }
        }

        private string ConvertOrderDetailsToMailTemplete(OrderInfoDetail order, ShoppingCart cart)
        {
            var result = new StringBuilder();
            foreach (var product in order.OrderProducts)
            {
                result.Append("<tr>");
                result.Append(string.Format("<td>{0}</td>", product.Item.ItemName));
                result.Append(string.Format("<td>{0}</td>", product.Quantity));
                result.Append(string.Format("<td>{0}</td>", product.GrossPrice));
                result.Append(string.Format("<td>{0}</td>", product.NetPrice * product.Quantity));
                result.Append("</tr>");
            }
            result.Append("<tr>");
            result.Append(string.Format("<td col=\"3\">{0}</td>", LanguageResource.Shipping));
            result.Append(string.Format("<td>{0}</td>", cart.ShipmentInfo != null ? cart.ShipmentInfo.TotalShippingFee : 0));
            result.Append("</tr>");
            result.Append("<tr>");
            result.Append(string.Format("<td col=\"3\">{0}</td>", LanguageResource.Promotion));
            result.Append(string.Format("<td>{0}</td>", cart.Promotion));
            result.Append("</tr>");
            result.Append("<tr>");
            result.Append(string.Format("<td col=\"3\">{0}</td>", LanguageResource.Total));
            result.Append(string.Format("<td>{0}</td>", cart.Total));
            result.Append("</tr>");
            return result.ToString();
        }

        #endregion

        #region Stripe

        private bool PaymentWithStripeCreditCard(Guid applicationId, CustomerInfoDetail customer, BillingAddressDetail address,
            TransactionRedirectUrls redirectUrls, OrderInfoDetail order, out string statePayment)
        {
            bool result = false;
            try
            {
                string custEmail = customer.Email;
                string custFullName = $"{customer.FirstName} {customer.LastName}";

                // Now make a transaction object and assign the Amount object
                var transDesc = new StringBuilder();
                transDesc.AppendFormat($"{LanguageResource.OrderNo} : {order.OrderNo}");
                transDesc.AppendFormat($"{LanguageResource.OrderDate} : {order.OrderDate}");
                transDesc.AppendFormat($"{LanguageResource.CustomerName} : {custFullName}");
                transDesc.AppendFormat($"{LanguageResource.Email} : {custEmail}");

                // Specify your total payment amount and assign the details object
                var subTotal = $"{order.SubTotal:0.00}";
                var shippingRate = $"{order.ShippingRate:0.00}";
                var tax = $"{order.Tax:0.00}";
                var total = $"{order.TotalFees:0.00}";
                var currency = order.CurrencyCode.ToLower();

                //var regionInfo = new RegionInfo(CultureInfo.CurrentCulture.LCID);
                //var currency = regionInfo.ISOCurrencySymbol;

                string publicKey = string.Empty, secretKey = string.Empty;
                var settings = ApplicationService.GetActiveStripeSettings(applicationId).ToList();
                if (settings.Any())
                {
                    publicKey = settings.FirstOrDefault(x => x.KeyName.ToLower() == "publickey")?.KeyValue;
                    secretKey = settings.FirstOrDefault(x => x.KeyName.ToLower() == "secretkey")?.KeyValue;

                }

                //Live
                //var stripeSetting = new StripeSettings
                //{
                //    PublishableKey = "pk_live_De8CUtE9hghwDHH5RjZd4ilW",
                //    SecretKey = "sk_live_SrMdDWgaM8T43NzxCWSPEQ5N"
                //};

                //Sandbox
                //var stripeSettings = new StripeSettings
                //{
                //    PublishableKey = "pk_test_4UczYxVDTWA0jRZSR1kfZvJZ",
                //    SecretKey = "sk_test_4ApIDhcocMTY68PcZgCWesc0"
                //};

                // Use Stripe's library to make request -  set your API key
                StripeConfiguration.SetApiKey(secretKey);

                //Securely collecting payment details from your customer
                var orderPayment = order.OrderPayment;
                // Get the credit card details submitted by the form
                var sourceToken = orderPayment.PaymentToken;

                //var card = new StripeCreditCardOptions
                //{
                //   // TokenId = orderPayment.PaymentToken,
                //    Name = orderPayment.CardHolder,
                //    Number = orderPayment.CardNo,
                //    ExpirationYear = orderPayment.ExpYear,
                //    ExpirationMonth = orderPayment.ExpMonth,
                //    Currency = currency,
                //    Cvc = orderPayment.Cvv
                //};

                //var tokenOptions = new StripeTokenCreateOptions
                //{
                //    CustomerId = order.CustomerId.ToString(),
                //    Card = card,
                //};

                //var tokenService = new StripeTokenService();
                //StripeToken stripeToken = tokenService.Create(tokenOptions);
                //string sourceToken = stripeToken.Id;

                ////you can create as many items as you want and add to this list
                //var items = new List<StripeOrderItemOptions>();
                //foreach (var orderProduct in order.OrderProducts)
                //{
                //    //create and item for which you are taking payment
                //    //if you need to add more items in the list
                //    //Then you will need to create multiple item objects or use some loop to instantiate object
                //    var price = Convert.ToInt32(orderProduct.GrossPrice);
                //    var item = new StripeOrderItemOptions
                //    {
                //        Type = "sku",
                //        Parent = orderProduct.Item.ItemCode,
                //        Amount = price,
                //        Currency = orderProduct.CurrencyCode,
                //        Description = transDesc.ToString(),
                //        Quantity = orderProduct.Quantity
                //    };
                //    items.Add(item);
                //}

                //var shipping = new StripeShippingOptions
                //{
                //    Name = custFullName,
                //    Line1 = address.Line1,
                //    CityOrTown = address.City,
                //    State = address.State, //"CA"
                //    Country = address.CountryCode,
                //    PostalCode = address.PostalCode,
                //};

                //var orderService = new StripeOrderService();
                //StripeOrder stripeOrder = orderService.Create(new StripeOrderCreateOptions
                //{
                //    Currency = currency,
                //    Items = items,
                //    Shipping = shipping,
                //    Coupon = order.PromotionCode,
                //    Email = custEmail
                //});
                //var TOKEN_URI = 'https://connect.stripe.com/oauth/token';
                //var AUTHORIZE_URI = 'https://connect.stripe.com/oauth/authorize';

                // Token is created using Checkout or Elements!
                // Get the payment token submitted by the form:
                // var token = model.Token; // Using ASP.NET MVC

                var customerOptions = new StripeCustomerCreateOptions()
                {
                    Email = customer.Email,
                    Description = string.Format("{0}{1}", customer.FirstName, customer.LastName),
                    SourceToken = sourceToken //# obtained with Stripe.js
                };

                // Create a Customer
                //var customers = new StripeCustomerService(secretKey);
                var customers = new StripeCustomerService();
                var stripeCustomer = customers.Create(customerOptions);

                var amount = order.TotalFees;
                // var regionInfo = new RegionInfo(CultureInfo.CurrentCulture.LCID);
                //token ID is created from Stripe.js e.g tok_KPte7942xySKBKyrBu11yEpf
                var chargeOptions = new StripeChargeCreateOptions()
                {
                  //  CustomerId = customer.CustomerId.ToString(),
                    Amount = Convert.ToInt32(amount),
                    Currency = currency,
                    Description = transDesc.ToString(),
                    SourceTokenOrExistingSourceId = sourceToken,
                    Metadata = new Dictionary<String, String>()
                    {
                        { "OrderId", order.OrderNo.ToString()}
                    }
                };

                var charges = new StripeChargeService(secretKey);
                var charge = charges.Create(chargeOptions);

                if (charge != null)
                {
                    var orderPaymentEditEntry = new OrderPaymentEditEntry
                    {
                        PaymentCode = charge.Id,
                        OrderNo = order.OrderNo,
                        Amount = order.TotalFees,
                        CustomerId = order.CustomerId,
                        PaymentMethodId = order.OrderPayment.PaymentMethodId,
                        CardType = order.OrderPayment.CardType,
                        CardHolder = order.OrderPayment.CardHolder,
                        CardNo = order.OrderPayment.CardNo,
                        ExpMonth = Convert.ToInt32(order.OrderPayment.ExpMonth),
                        ExpYear = Convert.ToInt32(order.OrderPayment.ExpYear)
                    };
                  
                    UpdateOrderPayment(orderPaymentEditEntry);
                    result = true;
                }
                
                statePayment = PaymentState.Approved;
                return result;
            }
            catch (StripeException e)
            {
                switch (e.StripeError.ErrorType)
                {
                    case "card_error":
                        statePayment = $"Card Error {e.StripeError.Code} : {e.StripeError.Message}";
                        Logger.Error(statePayment);
                        break;
                    case "api_connection_error":
                        statePayment = $"API Connection Error {e.StripeError.Code} : {e.StripeError.Message}";
                        Logger.Error(statePayment);
                        break;
                    case "api_error":
                        statePayment = $"API Error {e.StripeError.Code} : {e.StripeError.Message}";
                        Logger.Error(statePayment);
                        break;
                    case "authentication_error":
                        statePayment = $"Authentication Error {e.StripeError.Code} : {e.StripeError.Message}";
                        Logger.Error(statePayment);
                        break;
                    case "invalid_request_error":
                        statePayment = $"Invalid Request Error {e.StripeError.Code} : {e.StripeError.Message}";
                        Logger.Error(statePayment);
                        break;
                    case "rate_limit_error":
                        statePayment = $"Rate Limit Error {e.StripeError.Code} : {e.StripeError.Message}";
                        Logger.Error(statePayment);
                        break;
                    case "validation_error":
                        statePayment = $"Validation Error {e.StripeError.Code} : {e.StripeError.Message}";
                        Logger.Error(statePayment);
                        break;
                    default:
                        // Unknown Error Type
                        statePayment = $"Error {e.StripeError.Code} : {e.StripeError.Message}";
                        Logger.Error(statePayment);
                        break;
                }
                return false;
            }
        }
        #endregion

        #region PayPal

        private bool PaymentWithPayPal(Guid applicationId, OrderInfoDetail order, string payerId, out string statePayment)
        {
            bool result;
            var violations = new List<RuleViolation>();
            try
            {
                //Check whether customer exists or not
                var customer = UnitOfWork.CustomerRepository.FindById(order.CustomerId);
                if (customer == null)
                {
                    violations.Add(new RuleViolation(ErrorCode.NotFoundCustomer, "Customer", null, ErrorMessage.Messages[ErrorCode.NotFoundCustomer]));
                    throw new ValidationError(violations);
                }

                //you can create as many items as you want and add to this list
                var itemList = new ItemList();
                var items = new List<Item>();
                foreach (var orderProduct in order.OrderProducts)
                {
                    if (orderProduct.Quantity > 0)
                    {
                        //create and item for which you are taking payment
                        //if you need to add more items in the list
                        //Then you will need to create multiple item objects or use some loop to instantiate object
                        var price = $"{orderProduct.GrossPrice:0.00}";
                        var item = new Item
                        {
                            name = orderProduct.Item.ItemName,
                            currency = orderProduct.CurrencyCode,
                            price = price,
                            quantity = orderProduct.Quantity.ToString(),
                            sku = orderProduct.Item.ItemCode
                        };
                        items.Add(item);
                    }
                }
                itemList.items = items;

                // Now make a transaction object and assign the Amount object
                var transDesc = new StringBuilder();
                transDesc.AppendFormat($"{LanguageResource.OrderNo} : {order.OrderNo}");
                transDesc.AppendFormat($"{LanguageResource.OrderDate} : {order.OrderDate.ToString("MM/dd/yyyy hh:mm:ss")}");
                transDesc.AppendFormat($"{LanguageResource.CustomerName} : {customer.FirstName} {customer.LastName}");

                // Specify your total payment amount and assign the details object
                var subTotal = $"{order.SubTotal:0.00}";
                var shipping = $"{order.ShippingRate:0.00}";
                var tax = $"{order.Tax:0.00}";
                var total = $"{order.TotalFees:0.00}";

                //RedirectUrls - http://localhost:3000/cancel or http://localhost:3000/process or paypalObject.SiteURL + "?cancel=true" 
                var baseUrl = HttpContext.Current.Request.Url.Authority;
                if (baseUrl.EndsWith("/"))
                {
                    baseUrl = baseUrl.Substring(0, baseUrl.Length - 1);
                }

                var redirectUrls = new RedirectUrls
                {
                    cancel_url = $"{ HttpContext.Current.Request.Url.Scheme}://{baseUrl}/CustOrder/TransactionFailure",
                    return_url = $"{HttpContext.Current.Request.Url.Scheme}://{baseUrl}/CustOrder/TransactionSuccess"
                };

                // Specify details of your payment amount.
                var details = new Details // Specify details of your payment amount.
                {
                    subtotal = subTotal,
                    shipping = shipping,
                    tax = tax,
                    fee = total //Fee charged by PayPal.
                                //shipping_discount =0, // Amount being discounted for the shipping fee.
                                //insurance = 0, // Amount being charged for the insurance fee. Only supported when the `payment_method`
                                //gift_wrap =0, //Amount being charged as gift wrap fee.
                };

                // Specify your total payment amount and assign the details object
                var amount = new Amount
                {
                    currency = order.CurrencyCode,
                    total = total,
                    details = details
                };

                // Now create Payer object and assign the fundinginstrument list to the object
                var payer = new Payer { payment_method = "paypal" };

                var transaction = new Transaction
                {
                    amount = amount,
                    description = transDesc.ToString(),
                    item_list = itemList,
                    invoice_number = order.OrderNo.ToString()
                };

                // Now, we have to make a list of transaction and add the transactions object
                // to this list. You can create one or more object as per your requirements
                List<Transaction> transactions = new List<Transaction> {transaction};

                // finally create the payment object and assign the payer object & transaction list to it
                Payment payment = new Payment
                {
                   // id = HttpContext.Current.Request.Params["guid"],
                    intent = "sale", // A Payment Resource; create one with its intent set to `sale`, `authorize`, or `order`
                    payer = payer,
                    //payee = payee,
                    // Now, we have to make a list of transaction and add the transactions object to this list. You can create one or more object as per your requirements
                    transactions = transactions,
                    //create_time = DateTime.UtcNow.ToString("dd/MM/yyyy"),
                    redirect_urls = redirectUrls
                };

                //getting context from the paypal
                //basically we are sending the clientID and clientSecret key in this function
                //to the get the context from the paypal API to make the payment
                //for which we have created the object above.

                //Basically, apiContext object has a accesstoken which is sent by the paypal
                //to authenticate the payment to facilitator account.
                //An access token could be an alphanumeric string
                var paypalSetting = ApplicationService.GetActivePaypalSetting(applicationId);
                if (paypalSetting == null)
                {
                    violations.Add(new RuleViolation(ErrorCode.NotFoundPaypalSetting, "Paypal", null, ErrorMessage.Messages[ErrorCode.NotFoundPaypalSetting]));
                    throw new ValidationError(violations);
                }

                var config = new Dictionary<string, string>
                {
                    {"model", paypalSetting.Mode},
                    {"clientId", paypalSetting.ClientId},
                    {"clientSecret", paypalSetting.ClientSecret},
                    {"connectionTimeout", paypalSetting.ConnectionTimeout},
                    {"requestRetries", paypalSetting.RequestRetries}
                };

                var credential = new OAuthTokenCredential(paypalSetting.ClientId, paypalSetting.ClientSecret, config);
                string accessToken = credential.GetAccessToken();

                if (string.IsNullOrEmpty(accessToken))
                {
                    violations.Add(new RuleViolation(ErrorCode.UnableToAccessToken, "AccessToken", accessToken, ErrorMessage.Messages[ErrorCode.UnableToAccessToken]));
                    throw new ValidationError(violations);
                }

                //  Initialize the apiContext's configuration with the default configuration for this application. return apicontext object by invoking it with the accesstoken
                var apiContext = new APIContext(accessToken) { Config = config };

                if (string.IsNullOrEmpty(payerId))
                {
                    // Create a payment using a APIContext
                    var createdPayment = payment.Create(apiContext);
                }
                else
                {
                    // This section is executed when we have received all the payments parameters
                    // from the previous call to the function Create
                    // Executing a payment
                    var paymentExecution = new PaymentExecution { payer_id = payerId };
                    payment.Execute(apiContext, paymentExecution);
                }

                //if the createdPayment.state is "approved" it means the payment was successful else not
                statePayment = payment.state.ToLower();
                result = !string.IsNullOrEmpty(statePayment) && statePayment == PaymentState.Approved;
                return result;
            }
            catch (PayPal.PayPalException ex)
            {
                // ex.Details contains more detailed information about what error is being reported from the PayPal REST API
                statePayment = ex.ToString();
                result = !string.IsNullOrEmpty(statePayment) && statePayment == PaymentState.Approved;
                return result;
            }
        }
        private bool PaymentWithPayPalCreditCard(CustomerInfoDetail customer, BillingAddressDetail address,
            TransactionRedirectUrls redirectUrls, OrderInfoDetail order, out string statePayment)
        {
            bool result;
            var violations = new List<RuleViolation>();

            //you can create as many items as you want and add to this list
            var itemList = new ItemList();
            var items = new List<Item>();
            foreach (var orderProduct in order.OrderProducts)
            {
                //create and item for which you are taking payment
                //if you need to add more items in the list
                //Then you will need to create multiple item objects or use some loop to instantiate object
                var price = $"{orderProduct.GrossPrice:0.00}";
                var item = new Item
                {
                    name = orderProduct.Item.ItemName,
                    currency = orderProduct.CurrencyCode,
                    price = price,
                    quantity = orderProduct.Quantity.ToString(),
                    sku = orderProduct.Item.ItemCode
                };
                items.Add(item);
            }
            itemList.items = items;

            // Now make a transaction object and assign the Amount object
            var transDesc = new StringBuilder();
            transDesc.AppendFormat($"{LanguageResource.OrderNo} : {order.OrderNo}");
            transDesc.AppendFormat($"{LanguageResource.OrderDate} : {order.OrderDate}");
            //transDesc.AppendFormat($"{LanguageResource.CustomerName} : {customer.FirstName} {customer.LastName}");

            // Specify your total payment amount and assign the details object
            var subTotal = $"{order.SubTotal:0.00}";
            var shipping = $"{order.ShippingRate:0.00}";
            var tax = $"{order.Tax:0.00}";
            var total = $"{order.TotalFees:0.00}";
            
            //Address for the payment
            var billingAddress = new Address
            {
                country_code = address.CountryCode,
                city = address.City,
                state = address.State,
                line1 = order.Shipment.ReceiverAddress,
                postal_code = order.Shipment.PostalCode,
                phone = order.Shipment.ReceiverPhone
            };

            //Now Create an object of credit card and add above details to it
            var creditCard = new PayPal.Api.CreditCard
            {
                billing_address = billingAddress,
                first_name = customer.FirstName,
                last_name = customer.LastName,
                cvv2 = order.OrderPayment.Cvv,
                expire_month = Convert.ToInt32(order.OrderPayment.ExpMonth),
                expire_year = Convert.ToInt32(order.OrderPayment.ExpYear),
                type = order.OrderPayment.CardType, // credit card type here paypal allows 4 types - `visa`, `mastercard`, `discover`, `amex`
                number = order.OrderPayment.CardNo.Replace(" ", string.Empty), //enter your credit card number here
            };

            // Specify details of your payment amount.
            var details = new Details // Specify details of your payment amount.
            {
                subtotal = subTotal,
                shipping = shipping,
                tax = tax,
                fee = total //Fee charged by PayPal.
                //shipping_discount =0, // Amount being discounted for the shipping fee.
                //insurance = 0, // Amount being charged for the insurance fee. Only supported when the `payment_method`
                //gift_wrap =0, //Amount being charged as gift wrap fee.
            };

            // Specify your total payment amount and assign the details object
            var amount = new Amount
            {
                currency = order.CurrencyCode,
                total = total,
                details = details
            };

            var transaction = new Transaction
            {
                amount = amount,
                description = transDesc.ToString(),
                item_list = itemList,
                invoice_number = order.OrderNo.ToString()
            };

            // Now, we have to make a list of transaction and add the transactions object
            // to this list. You can create one or more object as per your requirements
            List<Transaction> transactions = new List<Transaction> { transaction };

            //Now we need to specify the FundingInstrument of the Payer for credit card payments, set the CreditCard which we made above
            var fundingInstruments = new List<FundingInstrument> // The Payment creation API requires a list of FundingIntrument
            {
                new FundingInstrument
                {
                    credit_card = creditCard
                }
            };

            // Now create Payer object and assign the fundinginstrument list to the object
            var payer = new Payer
            {
                payment_method = "credit_card", //or "paypal" 
                funding_instruments = fundingInstruments
            };

          
            // finally create the payment object and assign the payer object & transaction list to it
            var payment = new Payment
            {
                // id= order.OrderNo.ToString(),
                intent = "sale", // A Payment Resource; create one with its intent set to `sale`, `authorize`, or `order`
                payer = payer,
                //payee = payee,
                transactions = transactions,
                //create_time = createTime.ToString(CultureInfo.InvariantCulture.DateTimeFormat),
                redirect_urls = new RedirectUrls
                {
                    cancel_url = redirectUrls.CancelUrl,
                    return_url = redirectUrls.ReturnUrl
                }
            };

            try
            {
                var apiContext = PayPalService.GetApiContext();

                //Create is a Payment class function which actually sends the payment details
                //to the paypal API for the payment. The function is passed with the ApiContext
                //which we received above.
                // Create a payment using a valid APIContext
                // var createdPayment = Payment.Create(apiContext, payment);
                var createdPayment = payment.Create(apiContext);
                
                //// Get the authorization resource.
                //var authorization = createdPayment.transactions[0].related_resources[0].authorization;
                //var authorizationState = authorization.state;

                //// Check and make sure the card has been authorized for the payment.
                //if (authorizationState != "authorized")
                //{
                //    return;
                //}

                //// Specify an amount to capture.  By setting 'is_final_capture' to true, all remaining funds held by the authorization will be released from the funding instrument.
                //var capture = new Capture
                //{
                //    amount = new Amount
                //    {
                //        currency = "USD",
                //        total = "1.00"
                //    },
                //    is_final_capture = true
                //};

                //// Capture the payment.
                //var responseCapture = authorization.Capture(apiContext, capture);

                //if the createdPayment.state is "approved" it means the payment was successful else not
                statePayment = createdPayment.state.ToLower();
                result = !string.IsNullOrEmpty(statePayment) && statePayment == PaymentState.Created;
                return result;
            }
            catch (PayPal.PayPalException ex)
            {
                // ex.Details contains more detailed information about what error is being reported from the PayPal REST API
                //ex.InnerException.ToString();
                statePayment = ex.ToString();
                result = !string.IsNullOrEmpty(statePayment) && statePayment == PaymentState.Approved;
                return result;
            }
        }
        
        #endregion

        #region Dispose

        private bool _disposed;
        protected override void Dispose(bool isDisposing)
        {
            if (!_disposed)
            {
                if (isDisposing)
                {
                    ApplicationService = null;
                    VendorService = null;
                    CustomerService = null;
                    EmployeeService = null;
                    MailService = null;
                    MessageService = null;
                    NotificationService = null;
                }
                _disposed = true;
            }
            base.Dispose(isDisposing);
        }
        #endregion
    }
}
