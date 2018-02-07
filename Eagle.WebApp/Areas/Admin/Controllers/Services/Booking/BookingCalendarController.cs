using System;
using System.Linq;
using System.Web.Mvc;
using Eagle.Core.Settings;
using Eagle.Resources;
using Eagle.Services;
using Eagle.Services.Business;
using Eagle.Services.Dtos.Business.Transaction;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Dtos.Services.Booking;
using Eagle.Services.Service;
using Eagle.Services.Validations;
using Eagle.WebApp.Areas.Admin.Controllers.Common;

namespace Eagle.WebApp.Areas.Admin.Controllers.Services.Booking
{
    public class BookingCalendarController : BaseController
    {
        private IBookingService BookingService { get; set; }
        private IOrderService OrderService { get; set; }
        private ICurrencyService CurrencyService { get; set; }
        private ICustomerService CustomerService { get; set; }
        private IEmployeeService EmployeeService { get; set; }


        public BookingCalendarController(IBookingService bookingService, IOrderService orderService, ICurrencyService currencyService,
            ICustomerService customerService, IEmployeeService employeeService)
            : base(new IBaseService[] { bookingService, currencyService, customerService, employeeService })
        {
            BookingService = bookingService;
            OrderService = orderService;
            CurrencyService = currencyService;
            CustomerService = customerService;
            EmployeeService = employeeService;
        }

        #region Service Booking
        //GET: /Admin/BookingCalendar/Details/5        
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var item = OrderService.GetOrderProductDetails(id);

            var orderProduct = new OrderProductEditEntry
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
                Item = OrderService.GetOItemDetailsByProductId(item.ProductId, item.TypeId)
            };

            if (item.CustomerId != null && item.CustomerId > 0)
            {
                orderProduct.Customer = CustomerService.GetCustomerInfoDetail(Convert.ToInt32(item.CustomerId));
            }

            if (item.EmployeeId != null && item.EmployeeId > 0)
            {
                orderProduct.Employee = EmployeeService.GetEmployeeDetail(Convert.ToInt32(item.EmployeeId));
            }

            var startDate = Convert.ToDateTime(item.StartDate);
            var endDate = Convert.ToDateTime(item.EndDate);

            var fromPeriod = item.StartDate.HasValue && item.EndDate.HasValue ? (startDate - startDate.Date).TotalMinutes : 0;
            var toPeriod = item.StartDate.HasValue && item.EndDate.HasValue ? (endDate - endDate.Date).TotalMinutes : 0;
            
            ViewBag.ProductId = BookingService.PopulateServicePackSelectList(ServicePackStatus.Active, item.ProductId);
            ViewBag.PeriodGroupId = PoplulatePeriodGroupSelectList(orderProduct.PeriodGroupId);
            ViewBag.FromPeriod = BookingService.PopulatePeriod(Convert.ToInt32(fromPeriod));
            ViewBag.ToPeriod = BookingService.PopulatePeriod(Convert.ToInt32(toPeriod));

            return PartialView("../Services/Booking/BookingCalendar/_Edit", orderProduct);
        }


        public SelectList PoplulatePeriodGroupSelectList(int? selectedValue, bool? isShowSelectText = false)
        {
            var periodGroups = (from BookingPeriodGroup x in Enum.GetValues(typeof(BookingPeriodGroup))
                                select new SelectListItem
                                {
                                    Text = Enum.GetName(typeof(BookingPeriodGroup), x),
                                    Value = Convert.ToInt32(x).ToString()
                                }).ToList();
            
            if (isShowSelectText != null && isShowSelectText == true)
            {
                periodGroups.Insert(0, new SelectListItem { Text =
                    $"--- {LanguageResource.SelectPeriodGroup} ---", Value = "" });
            }
            return new SelectList(periodGroups,"Value", "Text", selectedValue); ;
        }

        #endregion

        #region GET METHODS =========================================================================

        // GET: Admin/BookingCalendar
        public ActionResult Index()
        {
            return View("../Services/Booking/BookingCalendar/Index");
        }
        
        public JsonResult GetCalendarEvents(BookingCalendarSearchEntry filter)
        {
            var events = BookingService.GetCalendarEvents(VendorId,ItemType.ServicePackage, filter.FromDate, filter.ToDate);
            var eventList = (from e in events
                             select new
                             {
                                 id = e.Id,
                                 title = e.Title,
                                 start = e.Start.ToString("s"),
                                 end = e.End.ToString("s"),
                                 color = e.Color,
                                 someKey = e.SomeKey,
                                 allDay = false
                             }).ToArray();

            return Json(eventList, JsonRequestBehavior.AllowGet);
        }

        // PUT - /Admin/BookingCalendar/Edit
        [HttpPut]
        public ActionResult Edit(OrderProductEditEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    OrderService.UpdateOrderProduct(entry);
                    return Json(new SuccessResult
                    {
                        Status = ResultStatusType.Success,
                        Data = new
                        {
                            Message = LanguageResource.UpdateSuccess,
                            Id = entry.ProductId
                        }
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var modeStateErrors = ModelState.GetModelStateErrors();
                    return Json(new FailResult { Status = ResultStatusType.Fail, Errors = modeStateErrors }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (ValidationError ex)
            {
                var errors = ValidationExtension.GetException(ex);
                return Json(new FailResult { Status = ResultStatusType.Fail, Errors = errors }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult PoplulateCustomerSelectList(string search, int? page)
        {
            int recordCount;
            var employees = CustomerService.GetCustomerAutoCompleteList(search, CustomerStatus.Published, out recordCount, "Id DESC", page);
            return Json(employees, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PoplulateEmployeeSelectList(int productId, int? selectedValue = null, bool? isShowSelectText = true)
        {
            var employees = BookingService.PopulateProviderSelectList(productId, null, selectedValue, isShowSelectText);
            return Json(employees, JsonRequestBehavior.AllowGet);
        }
        #endregion =====================================================================================

        #region Dispose

        private bool _disposed;

        [NonAction]
        protected override void Dispose(bool isDisposing)
        {
            if (!_disposed)
            {
                if (isDisposing)
                {
                    BookingService = null;
                    CurrencyService = null;
                    CustomerService = null;
                    EmployeeService = null;
                }
                _disposed = true;
            }
            base.Dispose(isDisposing);
        }

        #endregion
    }
}