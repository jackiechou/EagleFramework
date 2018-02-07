using Eagle.Core.Configuration;
using Eagle.Core.Pagination;
using Eagle.Core.Settings;
using Eagle.Services.Business;
using Eagle.Services.Common;
using Eagle.Services.Dtos.Business;
using Eagle.Services.Dtos.Common;
using Eagle.Services.SystemManagement;
using Eagle.Services.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Eagle.Common.Utilities;
using Eagle.Resources;
using Eagle.Services.Dtos.Business.Transaction;
using Eagle.Services.Service;

namespace Eagle.WebApp.Controllers
{
    public class CustOrderController : Controller
    {
        #region Contruct
        private IApplicationService ApplicationService { get; set; }
        private IBookingService BookingService { get; set; }
        private IOrderService OrderService { get; set; }
        private IProductService ProductService { get; set; }
        private ITransactionService TransactionService { get; set; }
        private ICurrencyService CurrencyService { get; set; }
        private IShippingService ShippingService { get; set; }
        private IContactService ContactService { get; set; }
        private IVendorService VendorService { get; set; }
        private ICustomerService CustomerService { get; set; }

        public CustOrderController(IApplicationService applicationService, ICustomerService customerService, IBookingService bookingService, IOrderService orderService, ITransactionService transactionService, IProductService productService, ICurrencyService currencyService, IShippingService shippingService, IContactService contactService, IVendorService vendorService)
        {
            ApplicationService = applicationService;
            CustomerService = customerService;
            BookingService = bookingService;
            OrderService = orderService;
            TransactionService = transactionService;
            ProductService = productService;
            CurrencyService = currencyService;
            ShippingService = shippingService;
            ContactService = contactService;
            VendorService = vendorService;
        }

        private bool _disposed;

        [NonAction]
        protected override void Dispose(bool isDisposing)
        {
            if (!_disposed)
            {
                if (isDisposing)
                {
                    ApplicationService = null;
                    BookingService = null;
                    OrderService = null;
                    ProductService = null;
                    TransactionService = null;
                    ProductService = null;
                    CurrencyService = null;
                    ShippingService = null;
                    ContactService = null;
                    VendorService = null;
                }
                _disposed = true;
            }
            base.Dispose(isDisposing);
        }
        #endregion

        #region GET - ORDERS

        [HttpGet]
        public ActionResult LoadSearchForm()
        {
            return PartialView("../Cust/_CustOrderSearchForm");
        }

        [HttpGet]
        public ActionResult Search(OrderSearchBaseEntry filter, int? page = 1)
        {
            if (SessionExtension.CustomerInfo == null)
            {
                return View("../Cust/Index");
            }

            var customerInfo = SessionExtension.CustomerInfo;
            var searchEntry = new OrderSearch
            {
                SearchText = filter.SearchText,
                VendorId = GlobalSettings.DefaultVendorId,
                CustomerId = customerInfo.CustomerId,
                Status = null
            };

            int? recordCount = 0;
            var sources = OrderService.GetOrdersByCustomer(searchEntry, ref recordCount, "OrderId DESC", page, GlobalSettings.DefaultPageSize).ToList();
            int currentPageIndex = page - 1 ?? 0;
            var lst = sources.ToPagedList(currentPageIndex, GlobalSettings.DefaultPageSize, recordCount);
            return PartialView("../Cust/_CustOrderSearchResult", lst);
        }

        #endregion

        #region GET - Transaction Methods - Shipping Methods - Payment Methods - Credit CardType

        [HttpGet]
        public ActionResult PopulateTransactionMethods()
        {
            var transactionMethods = TransactionService.GetTransactionMethods(TransactionMethodStatus.Active).ToList();
            return PartialView("../Order/_TransactionMethod", transactionMethods);
        }

        /// <summary>
        /// LOAD SHIPPING METHODS IN CHECKOUT FORM
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult PopulateShippingMethods()
        {
            var shippingMethods = ShippingService.GetShippingMethods(true).ToList();
            return PartialView("../Order/_ShippingMethod", shippingMethods);
        }

        public SelectList PopulateCardTypes(string selectedValue = "visa")
        {
            List<SelectListItem> lst = new List<SelectListItem>
            {
                new SelectListItem {Text = LanguageResource.Visa, Value = "visa"},
                new SelectListItem {Text = LanguageResource.MasterCard, Value = "mastercard"}
            };
            return new SelectList(lst, "Value", "Text", selectedValue);
        }
        
        /// <summary>
        /// LOAD PAYMENT METHODS
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult PopulatePaymentMethods()
        {
            var paymentMethods = TransactionService.GetPaymentMethods(PaymentMethodStatus.Active);
            return PartialView("../Order/_PaymentMethods", paymentMethods);
        }

        /// <summary>
        /// LOAD PAYMENT SETTINGS - CREDIT CARD IN CHECKOUT FORM
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Payment()
        {
            var payment = (OrderPaymentEntry)Session["OrderPayment"];
            if (payment == null)
            {
                var transactionState = new TransactionState
                {
                    Errors = new List<RuleViolation>
                    {
                        new RuleViolation(ErrorCode.NotFoundCustomerPayment, "OrderPayment", null,
                            ErrorMessage.Messages[ErrorCode.NotFoundCustomerPayment])
                    }
                };
                return RedirectToAction("TransactionFailure", "CustOrder", transactionState);
            }
            else
            {
                string paymentPath;
                var paymentMethodSetting = (PaymentMethodSetting)payment.PaymentMethodId;
                switch (paymentMethodSetting)
                {
                    case PaymentMethodSetting.Stripe:
                        var applicationId = GlobalSettings.DefaultApplicationId;
                        var settings = ApplicationService.GetActiveStripeSettings(applicationId).ToList();
                        if (settings.Any())
                        {
                            payment.PublicKey = settings.FirstOrDefault(x => x.KeyName.ToLower() == "publickey")?.KeyValue;
                            payment.BusinessName = settings.FirstOrDefault(x => x.KeyName.ToLower() == "businessmame")?.KeyValue;
                            payment.Description = $"{LanguageResource.OrderNo} : {payment.OrderNo}";
                        }
                        paymentPath = "../Order/PaymentStripe";
                        break;
                    case PaymentMethodSetting.Paypal:
                        paymentPath = "../Order/PaymentPayPal";
                        break;
                    case PaymentMethodSetting.Offline:
                        paymentPath = "../Order/PaymentOffline";
                        break;
                    default:
                        paymentPath = "../Order/PaymentOffline";
                        break;
                }

                payment.CardTypes = PopulateCardTypes();
                return View(paymentPath, payment);
            }
        }

        [HttpGet]
        public ActionResult TransactionSuccess()
        {
            var transaction = (TransactionInfo) TempData["TransactionInfo"];
            return View("../Order/TransactionSuccess", transaction);
        }

        [HttpGet]
        public ActionResult TransactionFailure()
        {
            var errors = TempData["TransactionException"] as List<Error>;
            return View("../Order/TransactionFailure", errors);
        }

        #endregion

        #region CHECK OUT - SHIPPING FEE - PROMOTION


        /// <summary>
        ///  GET: Create BILL - CART INFO
        /// List all selected items from shopping cart
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult CreateBill()
        {
            //Check whether the Customer has been logined/registered or not
            if (SessionExtension.CustomerInfo == null)
            {
                return Request.Url != null
                    ? Redirect("../Cust/Index?desiredUrl=" + HttpUtility.UrlDecode(Request.Url.ToString()))
                    : Redirect("../Cust/Index");
            }

            if (ShoppingCart.Instance == null || !ShoppingCart.Instance.Items.Any())
            {
                // Return to Home Page if Shopping cart is empty
                return RedirectToAction("Index", "Production");
            }
            
            var shoppingCheckOut = new ShoppingCheckOut
            {
                Cart = new CartInfoForShopping
                {
                    CustomerId = SessionExtension.CustomerInfo.CustomerId,
                    Weights = ShoppingCart.Instance.Weights,
                    Tax = ShoppingCart.Instance.Tax,
                    Discount = ShoppingCart.Instance.Discount,
                    Promotion = ShoppingCart.Instance.Promotion,
                    ShippingCharge = ShoppingCart.Instance.ShippingCharge,
                    SubTotal = ShoppingCart.Instance.SubTotal,
                    Total = ShoppingCart.Instance.Total,
                    CurrencyCode = ShoppingCart.Instance.CurrencyCode,
                    Count = ShoppingCart.Instance.Count,
                    Items = ShoppingCart.Instance.Items,
                    ShippingMethodId = ShoppingCart.Instance.ShippingMethodId,
                    ShipmentInfo = ShoppingCart.Instance.ShipmentInfo,
                    PromotionInfo = ShoppingCart.Instance.PromotionInfo,
                    CustomerInfo = SessionExtension.CustomerInfo
                }
            };

            var violations = new List<RuleViolation>();

            //Tao moi don hang neu chua co
            var orderEntry = new OrderEntry
            {
                CustomerId = SessionExtension.CustomerInfo.CustomerId,
                OrderDate = DateTime.UtcNow,
                DueDate = ShoppingCart.Instance.DueDate
            };

            var orderTemp = OrderService.InsertOrderTemp(GlobalSettings.DefaultVendorId, orderEntry);
            if (orderTemp == null)
            {
                violations.Add(new RuleViolation(ErrorCode.CannotSaveOrderTemp, "OrderTemp", orderEntry, ErrorMessage.Messages[ErrorCode.CannotSaveOrderTemp]));
                throw new ValidationError(violations);
            }

            //Gan gia tri Order No da tao cho phien dat hang
            var orderCode = Convert.ToString(orderTemp.OrderNo);
            ShoppingCart.Instance.OrderCode = orderCode;
            shoppingCheckOut.Cart.OrderCode = orderCode;

            //Luu productorder neu chua co
            foreach (var item in shoppingCheckOut.Cart.Items)
            {
                var orderProductTempEntry = new OrderProductTempEntry
                {
                    OrderNo = Guid.Parse(orderCode),
                    TypeId = item.TypeId,
                    ProductId = item.Id,
                    EmployeeId = item.EmployeeId,
                    Quantity = item.Quantity,
                    Weight = item.Weight,
                    NetPrice = item.NetPrice,
                    GrossPrice = item.GrossPrice,
                    TaxRate = item.TaxRate,
                    DiscountRate = item.DiscountRate,
                    CurrencyCode = item.CurrencyCode,
                    StartDate = item.StartDate,
                    EndDate = item.EndDate,
                    PeriodGroupId = item.PeriodGroupId,
                    FromPeriod = item.FromPeriod,
                    ToPeriod = item.ToPeriod,
                    Comment = item.Comment
                };

                var orderProductTemp = OrderService.InsertOrderProductTemp(orderProductTempEntry);
                if (orderProductTemp == null)
                {
                    violations.Add(new RuleViolation(ErrorCode.CannotSaveOrderProductTemp, "OrderProductTemp", orderProductTempEntry, ErrorMessage.Messages[ErrorCode.CannotSaveOrderProductTemp]));
                    throw new ValidationError(violations);
                }
            }

            //Update subtotal
            var orderEditEntry = new OrderEntry
            {
                CustomerId = shoppingCheckOut.Cart.CustomerId,
                SubTotal = shoppingCheckOut.Cart.SubTotal,
                TotalFees = shoppingCheckOut.Cart.Total,
                Discount = shoppingCheckOut.Cart.Discount,
                CurrencyCode = shoppingCheckOut.Cart.CurrencyCode
            };

            OrderService.UpdateOrderTemp(Guid.Parse(shoppingCheckOut.Cart.OrderCode), orderEditEntry);
            
            return View("../Order/CheckOut", shoppingCheckOut);
        }

        [HttpPost]
        public ActionResult CheckOut(CheckOutSubmitViewModel model)
        {
            try
            {
                if (SessionExtension.CustomerInfo == null)
                {
                    return Request.Url != null ? Redirect("../Cust/Index?desiredUrl=" + HttpUtility.UrlDecode(Request.Url.ToString())) : Redirect("../Cust/Index");
                }

                var shippingRate = ShoppingCart.Instance.ShipmentInfo != null
                        ? ShoppingCart.Instance.ShipmentInfo.TotalShippingFee
                        : 0;

                var applicationId = GlobalSettings.DefaultApplicationId;
                int vendorId = GlobalSettings.DefaultVendorId;
                var totalFees = ShoppingCart.Instance.Total;

                var dueDateRangeOfOrder = ApplicationService.GetOrderSetting(applicationId, OrderSetting.DueDateRange).Setting.KeyValue;
                var dueDate = DateTime.UtcNow.AddDays(Convert.ToInt32(dueDateRangeOfOrder));

                var shipDateRangeOfOrder = ApplicationService.GetOrderSetting(applicationId, OrderSetting.ShipDateRange).Setting.KeyValue;
                var shippingDate = DateTime.UtcNow.AddDays(Convert.ToInt32(shipDateRangeOfOrder));
                
                var customer = SessionExtension.CustomerInfo;
                int customerId = SessionExtension.CustomerInfo.CustomerId;
                
                var orderProducts = new List<OrderProductInfoDetail>();
                var orderProductItems = ShoppingCart.Instance.Items;
                if (orderProductItems.Any())
                {
                    foreach (var orderProductItem in orderProductItems)
                    {
                        var employeeId = orderProductItem.EmployeeId;
                        if (employeeId==null)
                        {
                            if (orderProductItem.TypeId == ItemType.ServicePackage)
                            {
                                var employees = BookingService.GetServicePackProviders(orderProductItem.Id);
                                if (employees != null && employees.Any())
                                {
                                    var employee = employees.FirstOrDefault();
                                    if (employee != null)
                                    {
                                        employeeId = employee.EmployeeId;
                                    }
                                    else
                                    {
                                        employeeId = 1;
                                    }
                                }
                                else
                                {
                                    employeeId = 1;
                                }
                            }
                            else
                            {
                                employeeId = 1;
                            }
                        }

                        var orderProductEntry = new OrderProductInfoDetail
                        {
                            OrderNo = Guid.Parse(ShoppingCart.Instance.OrderCode),
                            TypeId = orderProductItem.TypeId,
                            CustomerId = customerId,
                            EmployeeId = employeeId,
                            ProductId = orderProductItem.Id,
                            Quantity = orderProductItem.Quantity,
                            Weight = orderProductItem.Weight,
                            NetPrice = orderProductItem.NetPrice,
                            GrossPrice = orderProductItem.GrossPrice,
                            TaxRate = orderProductItem.TaxRate,
                            DiscountRate = orderProductItem.DiscountRate,
                            CurrencyCode = orderProductItem.CurrencyCode,
                            StartDate = orderProductItem.StartDate,
                            EndDate = orderProductItem.EndDate,
                            PeriodGroupId = orderProductItem.PeriodGroupId ?? 1,
                            FromPeriod = orderProductItem.FromPeriod,
                            ToPeriod = orderProductItem.ToPeriod,
                            Comment = orderProductItem.Comment,
                            Status = OrderProductStatus.Active,
                            CreatedDate = DateTime.UtcNow,
                            LastModifiedDate = DateTime.UtcNow,
                            Item = orderProductItem.Detail
                        };
                        orderProducts.Add(orderProductEntry);
                    }
                }

                var order = new OrderTransactionEntry
                {
                    CustomerId = customerId,
                    OrderNo = Guid.Parse(ShoppingCart.Instance.OrderCode),
                    CurrencyCode = ShoppingCart.Instance.CurrencyCode,
                    Discount = ShoppingCart.Instance.Discount,
                    SubTotal = ShoppingCart.Instance.SubTotal,
                    Tax = ShoppingCart.Instance.Tax,
                    TransactionMethodId = model.TransactionMethodId,
                    PromotionCode = model.PromotionInfo != null ? model.PromotionInfo.PromotionCode : string.Empty,
                    ShippingRate = shippingRate,
                    OrderDate = DateTime.UtcNow,
                    DueDate = dueDate,
                    CreatedDate = DateTime.UtcNow,
                    LastModifiedDate = DateTime.UtcNow,
                    Comment = model.CustomerInfo.Comment,
                    TotalFees = totalFees,
                    VendorId = vendorId,
                    Status = OrderStatus.Approved,
                    Shipment = new OrderShipmentEntry
                    {
                        OrderNo = Guid.Parse(ShoppingCart.Instance.OrderCode),
                        Weight = ShoppingCart.Instance.Weights,
                        CustomerId = customerId,
                        ReceiverEmail = model.CustomerInfo.Email,
                        ReceiverPhone = model.CustomerInfo.Phone,
                        ReceiverName = model.ShipmentInfo.ReceiverName,
                        ReceiverAddress = model.ShipmentInfo.AddressDetail,
                        PostalCode = model.ShipmentInfo.PostalCode,
                        ShippingMethodId = model.ShipmentInfo.ShippingMethodId,
                        CountryId = model.ShipmentInfo != null ? model.ShipmentInfo.CountryId : null,
                        CityId = model.ShipmentInfo != null ? model.ShipmentInfo.CityId : null,
                        RegionId = model.ShipmentInfo != null ? model.ShipmentInfo.RegionId : null,
                        ProvinceId = model.ShipmentInfo != null ? model.ShipmentInfo.ProvinceId : null,
                        IsInternational = false,
                        ShipDate = shippingDate
                    },
                    Payment = model.PaymentInfo,
                    OrderProducts = orderProducts
                };

                // Step 1: Insert into Db
                var transactionState = OrderService.PurchaseOrder(applicationId, vendorId, order);

                // Step 2: Process Payment
                if (transactionState.Errors != null && transactionState.Errors.Any())
                {
                    return RedirectToAction("TransactionFailure", "CustOrder", transactionState.Errors);
                }
                else
                {
                    //Process Transaction - Payment
                    order.OrderId = transactionState.Order.OrderId;
                    order.Payment.Amount = order.TotalFees;
                    order.Payment.OrderNo = order.OrderNo;
                    order.Payment.CustomerId = order.CustomerId;
                    
                    Session["OrderPayment"] = order.Payment;

                    return RedirectToAction("Payment", "CustOrder");
                }
            }
            catch (ValidationError ex)
            {
                TempData["TransactionException"] = ValidationExtension.GetException(ex);
              // return View("../Order/TransactionFailure", errors);
                return RedirectToAction("TransactionFailure", "CustOrder");
            }
        }


        [HttpPost]
        public ActionResult ProcessPayment(OrderPaymentEntry orderEntry)
        {
            try
            {
                //Process Transaction - Payment
                var applicationId = GlobalSettings.DefaultApplicationId;

                var result = OrderService.ProcessPayment(applicationId, orderEntry, out var statePayment);
                if (result)
                {
                    ShoppingCart.Instance.Clear();
                    var customer = CustomerService.GetCustomerInfoDetail(orderEntry.CustomerId);
                    var transaction = new TransactionInfo
                    {
                        FirstName = customer.FirstName,
                        LastName = customer.LastName,
                        Email = customer.Email,
                        OrderNo = orderEntry.OrderNo
                    };
                    TempData["TransactionInfo"] = transaction;

                    return RedirectToAction("TransactionSuccess", "CustOrder");
                }
                else
                {
                    var errors = new List<Error>
                    {
                        new Error
                        {
                            ErrorCode = ErrorCode.PaypalTransactionFailure,
                            ErrorMessage = statePayment,
                            ExtraInfos = new List<RuleViolation>
                            {
                                new RuleViolation(ErrorCode.NotFoundCustomerPayment, "Payment", null,
                                    statePayment)
                            }
                        }
                    };

                    TempData["TransactionException"] = errors;
                    
                    OrderService.UpdateOrderStatus(orderEntry.OrderNo, OrderStatus.Cancelled);
                    return RedirectToAction("TransactionFailure", "CustOrder");
                }

            }
            catch (ValidationError ex)
            {
                ShoppingCart.Instance.Clear();
                TempData["TransactionException"] = ValidationExtension.GetException(ex);
                // return View("../Order/TransactionFailure", errors);
                return RedirectToAction("TransactionFailure", "CustOrder");
            }
        }

        [HttpPost]
        public JsonResult CalculateShippingFee(int shippingMethodId, string zipCode)
        {
            try
            {
                //TODO Get Selected Shipping Carrier by vendorId
                int shippingCarrierId = 1;
                var shipmentRequest = new ShippingFeeSearchZipCodeEntry
                {
                    ShippingCarrierId = shippingCarrierId,
                    ShippingMethodId = shippingMethodId,
                    ZipCode = zipCode,
                    TotalWeight = ShoppingCart.Instance.Weights
                };

                var shipmentInfo = ShippingService.GetShipmentInfo(shipmentRequest);

                // Update Shopping Cart 
                ShoppingCart.Instance.ShipmentInfo = shipmentInfo;
                return Json(new SuccessResult
                {
                    Status = ResultStatusType.Success,
                    Data = new CheckoutShipmentInfoViewModel
                    {
                        ShipmentInfo = shipmentInfo,
                        Total = ShoppingCart.Instance.Total
                    }
                }, JsonRequestBehavior.AllowGet);
            }
            catch (ValidationError ex)
            {
                var errors = ValidationExtension.GetException(ex);
                return Json(new FailResult { Status = ResultStatusType.Fail, Errors = errors }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult ApplyPromotionCode(string promoCode)
        {
            try
            {
                // Calculate based on 
                OrderService.ApplyPromotion(1, promoCode, ShoppingCart.Instance);
                var result = new CheckOutPromotionViewModel
                {
                    Promotion = ShoppingCart.Instance.Promotion,
                    Total = ShoppingCart.Instance.Total
                };
                return Json(new SuccessResult
                {
                    Status = ResultStatusType.Success,
                    Data = result
                }, JsonRequestBehavior.AllowGet);
            }
            catch (ValidationError ex)
            {
                var errors = ValidationExtension.GetException(ex);
                return Json(new FailResult { Status = ResultStatusType.Fail, Errors = errors }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult RemovePromotionCode(string promoCode)
        {
            try
            {
                // Calculate based on 
                OrderService.RemovePromotion(ShoppingCart.Instance);
                var result = new CheckOutPromotionViewModel
                {
                    Promotion = ShoppingCart.Instance.Promotion,
                    Total = ShoppingCart.Instance.Total
                };
                return Json(new SuccessResult
                {
                    Status = ResultStatusType.Success,
                    Data = result
                }, JsonRequestBehavior.AllowGet);
            }
            catch (ValidationError ex)
            {
                var errors = ValidationExtension.GetException(ex);
                return Json(new FailResult { Status = ResultStatusType.Fail, Errors = errors }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion
    }
}