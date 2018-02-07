using Eagle.Core.Configuration;
using Eagle.Core.Settings;
using Eagle.Repositories;
using Eagle.Services.Common;
using Eagle.Services.Dtos.Business;
using Eagle.Services.SystemManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Eagle.Services.Dtos.Business.Transaction;
using Eagle.Services.Messaging;
using Eagle.Services.Service;
using Eagle.Services.Skins;

namespace Eagle.Services.Business
{
    public class ShoppingCart
    {
        private IApplicationService ApplicationService { get; set; }
        private IUserService UserService { get; set; }
        private IMailService MailService { get; set; }
        private INotificationService NotificationService { get; set; }
        private IBookingService BookingService { get; set; }
        private IProductService ProductService { get; set; }
        private IOrderService OrderService { get; set; }
        private IPayPalService PayPalService { get; set; }

        #region Properties

        // Chứa các mặt hàng đã chọn
        public List<CartItemForShopping> Items { get; private set; }
        public string OrderCode { get; set; }
        public DateTime OrderDate { get;set; } = DateTime.UtcNow;
        public DateTime? DueDate { get; set; }
        public int Count => Items.Count;
        public decimal Weights
        {
            get
            {
                if (ShipmentInfo != null)
                {

                }
                return Items.Sum(p => p.Weight ?? 0 * p.Quantity);
            }
        }
        public decimal Tax
        {
            get
            {
                return Items.Sum(p => (p.TaxRate ?? 0) * p.Quantity);
            }
        }
        public decimal Discount
        {
            get
            {
                return Items.Sum(p => (p.DiscountRate ?? 0) * p.Quantity);
            }
        }
        public decimal Promotion
        {
            get
            {
                decimal result = 0;
                if (PromotionInfo != null)
                {
                    var grossTotal = Items.Sum(p => p.GrossPrice * p.Quantity) ?? 0;
                    // Percent  type
                    if (PromotionInfo.IsPercent)
                    {
                        result = PromotionInfo.PromotionValue * grossTotal / 100;
                    }
                    else
                    {
                        // Promo Vallue type
                        result = PromotionInfo.PromotionValue;
                    }
                }
                return result;
            }
        }

        public decimal ShippingCharge
        {
            get
            {
                decimal shippingFee = ShipmentInfo != null ? ShipmentInfo.TotalShippingFee : 0;
                return shippingFee;
            }
        }
        public decimal SubTotal
        {
            get
            {
                var result = Items.Where(m=>m.Quantity>0).Sum(p => p.NetPrice* p.Quantity);
                // If Promotion is bigger than Gross
                if (result < 0)
                {
                    result = 0;
                }
                return result ?? 0;
            }
        }
        public decimal Total
        {
            get
            {
                var result = (SubTotal + Tax + ShippingCharge) - (Discount + Promotion);
                return result;
            }
        }
        public string CurrencyCode
        {
            get
            {
                if (Items != null && Items.Any())
                {
                    return Items[0].CurrencyCode;
                }
                else
                {
                    return GlobalSettings.DefaultCurrencyCode;
                }
            }
        }
        public int? ShippingMethodId { get; set; }
        public int? TransactionMethodId { get; set; }
        public PromotionInfo PromotionInfo { get; set; }
        public ShipmentInfo ShipmentInfo { get; set; }

        #endregion

        #region Singleton Implementation     

        // Hàm khởi tạo
        public static ShoppingCart Instance
        {
            get
            {
                if (HttpContext.Current.Session["Cart"] == null)
                {
                    // we are creating a local variable and thus not interfering with other users sessions
                    ShoppingCart instance = new ShoppingCart { Items = new List<CartItemForShopping>() };
                    HttpContext.Current.Session["Cart"] = instance;
                    return instance;
                }
                else
                {
                    // we are returning the shopping cart for the given user
                    return (ShoppingCart)HttpContext.Current.Session["Cart"];
                }
            }
        }

        public ShoppingCart()
        {
            IUnitOfWork unitOfWork = new UnitOfWork();
            ICacheService cacheService = new CacheService(unitOfWork);
            IDocumentService documentService = new DocumentService(unitOfWork);
            ILanguageService languageService = new LanguageService(unitOfWork);
            ILogService logService = new LogService(unitOfWork);
            IThemeService themeService = new ThemeService(unitOfWork, cacheService, documentService);
            IContactService contactService = new ContactService(unitOfWork, documentService);
            IVendorService vendorService = new VendorService(unitOfWork, contactService, documentService);

            IMessageService messageService = new MessageService(unitOfWork);
            ICurrencyService currencyService = new CurrencyService(unitOfWork);
            IEmployeeService employeeService = new EmployeeService(unitOfWork, vendorService, contactService, documentService);
            ICustomerService customerService = new CustomerService(unitOfWork, cacheService, contactService, documentService);
            IRoleService roleService = new RoleService(unitOfWork);
           
            ApplicationService = new ApplicationService(unitOfWork, cacheService, languageService, logService, themeService, vendorService);
            UserService = new UserService(unitOfWork, ApplicationService, currencyService, contactService, documentService, roleService);
            MailService = new MailService(unitOfWork, ApplicationService, messageService);
            NotificationService = new NotificationService(unitOfWork, MailService, messageService, UserService, customerService, employeeService);
            ProductService = new ProductService(unitOfWork, ApplicationService, vendorService, documentService, currencyService);
            PayPalService = new PayPalService(unitOfWork, ApplicationService);
            OrderService = new OrderService(unitOfWork,ApplicationService,vendorService, employeeService, customerService, MailService, messageService, NotificationService, PayPalService);
            BookingService = new BookingService(unitOfWork, ApplicationService, OrderService, customerService, employeeService, currencyService, contactService, documentService, MailService, messageService, NotificationService);        
        }

        #endregion

        /// <summary>
        ///  Booking an item in cart
        /// </summary>
        /// <param name="type"></param>
        /// <param name="applicationId"></param>
        /// <param name="id"></param>
        /// <param name="quantity"></param>
        /// <param name="employeeId"></param>
        /// <param name="periodGroupId"></param>
        /// <param name="fromPeriod"></param>
        /// <param name="toPeriod"></param>
        /// <param name="comment"></param>
        public void Add(Guid applicationId, int id, int quantity, ItemType type, int? employeeId =null, int? periodGroupId = null, int? fromPeriod = null, int? toPeriod = null, string comment = null)
        {
            var dueDateRangeOfOrder = ApplicationService.GetOrderSetting(applicationId, OrderSetting.DueDateRange).Setting.KeyValue;
            var dueDate = DateTime.UtcNow.AddDays(Convert.ToInt32(dueDateRangeOfOrder));

            //Initiate Cart Item
            var item = Items.FirstOrDefault(i => i.Id == id);
            if (item != null)
            {
                item.Quantity += quantity;

                var weight = item.Weight * item.Quantity;
                item.Weight = weight;
                item.Customer = SessionExtension.CustomerInfo;
            }
            else // chưa có trong giỏ -> truy vấn CSDL và bỏ vào giỏ
            {
                if (type == ItemType.Product)
                {
                    ProductInfoDetail productInfo = ProductService.GetProductDetails(id);
                    if (productInfo == null) return;

                    var cartItem = new CartItemForShopping(productInfo.ProductId)
                    {
                        Id = id,
                        TypeId = type,
                        Quantity = quantity,
                        EmployeeId = employeeId,
                        PeriodGroupId = periodGroupId,
                        FromPeriod = fromPeriod,
                        ToPeriod = toPeriod,
                        StartDate = DateTime.UtcNow,
                        EndDate = dueDate,
                        Name = productInfo.ProductName,
                        Weight = productInfo.Weight ?? 0,
                        NetPrice = productInfo.NetPrice ?? 0,
                        GrossPrice = productInfo.GrossPrice ?? 0,
                        TaxRate = productInfo.TaxRate ?? 0,
                        DiscountRate = productInfo.DiscountRate ?? 0,
                        CurrencyCode = productInfo.CurrencyCode,
                        Image = productInfo.SmallPhotoUrl,
                        UnitsInStock = productInfo.UnitsInStock ?? 0,
                        Status = CartItemStatus.Available,
                        Detail = new ItemDetail
                        {
                            TypeId = type,
                            ItemId = productInfo.ProductId,
                            ItemCode = productInfo.ProductCode,
                            ItemAlias = productInfo.ProductAlias,
                            ItemName = productInfo.ProductName,
                            UnitsInStock = productInfo.UnitsInStock,
                            Weight = productInfo.Weight,
                            NetPrice = productInfo.NetPrice,
                            GrossPrice = productInfo.GrossPrice,
                            TaxRate = productInfo.TaxRate,
                            DiscountRate = productInfo.DiscountRate,
                            CurrencyCode = productInfo.CurrencyCode
                        }
                    };

                    Items.Add(cartItem);
                }

                if (type == ItemType.ServicePackage)
                {
                    var packageInfo = BookingService.GetServicePackDetail(id);
                    if (packageInfo == null) return;

                    var cartItem = new CartItemForShopping(packageInfo.PackageId)
                    {
                        Id = id,
                        TypeId = type,
                        Quantity = quantity,
                        EmployeeId = employeeId,
                        PeriodGroupId = periodGroupId,
                        FromPeriod = fromPeriod,
                        ToPeriod = toPeriod,
                        StartDate = DateTime.UtcNow,
                        EndDate = dueDate,
                        Comment = comment,
                        Name = packageInfo.PackageName,
                        Weight = packageInfo.Weight ?? 0,
                        NetPrice = packageInfo.PackageFee ?? 0,
                        GrossPrice = packageInfo.TotalFee ?? 0,
                        TaxRate = packageInfo.TaxRate ?? 0,
                        DiscountRate = packageInfo.DiscountRate ?? 0,
                        CurrencyCode = packageInfo.CurrencyCode,
                        Image = packageInfo.FileUrl,
                        UnitsInStock = packageInfo.AvailableQuantity ?? 0,
                        Status = CartItemStatus.Available,
                        Detail = new ItemDetail
                        {
                            TypeId = type,
                            ItemId = packageInfo.PackageId,
                            ItemCode = packageInfo.PackageCode,
                            ItemName = packageInfo.PackageName,
                            UnitsInStock = packageInfo.AvailableQuantity,
                            Weight = packageInfo.Weight,
                            NetPrice = packageInfo.PackageFee,
                            GrossPrice = packageInfo.TotalFee,
                            TaxRate = packageInfo.TaxRate,
                            DiscountRate = packageInfo.DiscountRate,
                            CurrencyCode = packageInfo.CurrencyCode,
                            EmployeeId = employeeId,
                            PeriodGroupId = periodGroupId,
                            FromPeriod = fromPeriod,
                            ToPeriod = toPeriod
                        }
                    };

                    Items.Add(cartItem);
                }
            }
        }

        /// <summary>
        /// Remove item in cart
        /// </summary>
        /// <param name="id"></param>
        public void Remove(int id)
        {
            if (Items != null && Items.ToList().Any())
            {
                var item = Items.FirstOrDefault(i => i.Id == id);
                Items.Remove(item);
            }
        }

        /// <summary>
        /// Change quantity in cart
        /// </summary>
        /// <param name="id"></param>
        /// <param name="newQuantity"></param>
        public void Update(int id, int newQuantity)
        {
            //if quantity equals 0, item is removed
            if (newQuantity == 0)
            {
                Remove(id);
                return;
            }

            //Find and update quantity of item in shopping cart
            if (Items == null || !Items.Any()) return;

            var item = Items.Single(i => i.Id == id);
            item.Quantity = newQuantity;
        }

        /// <summary>
        /// Remove All
        /// </summary>
        public void Clear()
        {
            Items.Clear();
        }
    }
}