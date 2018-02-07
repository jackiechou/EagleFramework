using System;
using System.Web.Mvc;
using Eagle.Core.Configuration;
using Eagle.Core.Pagination;
using Eagle.Core.Settings;
using Eagle.Resources;
using Eagle.Services;
using Eagle.Services.Business;
using Eagle.Services.Dtos.Business.Transaction;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Validations;
using Eagle.WebApp.Areas.Admin.Controllers.Common;

namespace Eagle.WebApp.Areas.Admin.Controllers.Business.Ecommerce
{
    public class OrderController : BaseController
    {
        private IOrderService OrderService { get; set; }
        private IProductService ProductService { get; set; }
        private ICurrencyService CurrencyService { get; set; }

        public OrderController(IOrderService orderService, IProductService productService, ICurrencyService currencyService)
            : base(new IBaseService[] { orderService, productService, currencyService })
        {
            OrderService = orderService;
            ProductService = productService;
            CurrencyService = currencyService;
        }

        // GET: Admin/Order
        public ActionResult Index()
        {
            return View("../Business/Ecommerce/Order/Index");
        }
        //// GET: /Admin/Product/Details/5        
        //[HttpGet]
        //public ActionResult Edit(int id)
        //{
        //    var item = OrderService.GetOrderDetail(id);

        //    var model = new OrderEditEntry();
        //    //{
        //    //    ProductId = item.ProductId,
        //    //    ProductCode = item.ProductCode,
        //    //    CategoryId = item.CategoryId,
        //    //    ManufacturerId = item.ManufacturerId,
        //    //    ProductTypeId = item.ProductTypeId,
        //    //    ProductName = item.ProductName,
        //    //    DiscountId = item.DiscountId,
        //    //    VendorId = item.VendorId,
        //    //    CurrencyCode = item.CurrencyCode,
        //    //    NetPrice = item.NetPrice,
        //    //    GrossPrice = item.GrossPrice,
        //    //    TaxRateId = item.TaxRateId,
        //    //    UnitsInStock = item.UnitsInStock,
        //    //    UnitsOnOrder = item.UnitsOnOrder,
        //    //    UnitsInAPackage = item.UnitsInAPackage,
        //    //    UnitsInBox = item.UnitsInBox,
        //    //    Unit = item.Unit,
        //    //    Weight = item.Weight,
        //    //    UnitOfWeightMeasure = item.UnitOfWeightMeasure,
        //    //    Length = item.Length,
        //    //    Width = item.Width,
        //    //    Height = item.Height,
        //    //    UnitOfDimensionMeasure = item.UnitOfDimensionMeasure,
        //    //    Url = item.Url,
        //    //    MinPurchaseQty = item.MinPurchaseQty,
        //    //    MaxPurchaseQty = item.MaxPurchaseQty,
        //    //    Views = item.Views,
        //    //    SmallPhoto = item.SmallPhoto,
        //    //    LargePhoto = item.LargePhoto,
        //    //    ShortDescription = item.ShortDescription,
        //    //    Specification = HttpUtility.HtmlDecode(item.Specification),
        //    //    Availability = item.Availability,
        //    //    StartDate = item.StartDate,
        //    //    EndDate = item.EndDate,
        //    //    PurchaseScope = item.PurchaseScope,
        //    //    Warranty = item.Warranty,
        //    //    IsOnline = item.IsOnline,
        //    //    InfoNotification = item.InfoNotification,
        //    //    PriceNotification = item.PriceNotification,
        //    //    QtyNotification = item.QtyNotification,
        //    //    Status = item.Status
        //    //};

        //    //if (item.SmallPhoto != null)
        //    //{
        //    //    var fileInfo = DocumentService.GetFileInfoDetail((int)item.SmallPhoto);
        //    //    if (fileInfo != null)
        //    //    {
        //    //        model.FileUrl = fileInfo.FileUrl;
        //    //    }
        //    //}

        //    //ViewBag.CurrencyCode = CurrencyService.GetSelectedCurrency().CurrencyCode;
        //    //ViewBag.ManufacturerId = SupplierService.PoplulateManufacturerCategorySelectList(ManufacturerCategoryStatus.Active, item.ManufacturerId);
        //    //ViewBag.DiscountId = ProductService.PopulateProductDiscountSelectList(DiscountType.Normal, ProductDiscountStatus.Active, item.DiscountId);
        //    //ViewBag.TaxRateId = ProductService.PopulateProductTaxRateSelectList(ProductTaxRateStatus.Active, item.TaxRateId);
        //    return PartialView("../Business/Ecommerce/Order/_Edit", model);
        //}
        //// GET: /Admin/Order/Search
        //[HttpGet]
        //public ActionResult LoadSearchForm()
        //{
        //    return PartialView("../Business/Ecommerce/Order/_SearchForm");
        //}

        // GET: /Admin/Order/Search

        [HttpGet]
        public ActionResult Search(OrderSearchEntry filter, string sourceEvent, int? page = 1)
        {
            if (string.IsNullOrEmpty(sourceEvent))
            {
                TempData["OrderSearchRequest"] = filter;
            }
            else
            {
                if (TempData["OrderSearchRequest"] != null)
                {
                    filter = (OrderSearchEntry)TempData["OrderSearchRequest"];
                }
            }
            TempData.Keep();

            int? recordCount = 0;
            var sources = OrderService.GetOrders(VendorId, filter, ref recordCount, "OrderId DESC", page, GlobalSettings.DefaultPageSize);
            int currentPageIndex = page - 1 ?? 0;
            var lst = sources.ToPagedList(currentPageIndex, GlobalSettings.DefaultPageSize, recordCount);
            return PartialView("../Business/Ecommerce/Order/_SearchResult", lst);
        }

        //
        // POST: /Admin/Order/UpdateStatus/5
        [HttpPut]
        public ActionResult UpdateStatus(Guid orderNo, OrderStatus status)
        {
            try
            {
                OrderService.UpdateOrderStatus(orderNo, status);
                return Json(new SuccessResult { Status = ResultStatusType.Success, Data = LanguageResource.UpdateStatusSuccess }, JsonRequestBehavior.AllowGet);
            }
            catch (ValidationError ex)
            {
                var errors = ValidationExtension.GetException(ex);
                return Json(new FailResult { Status = ResultStatusType.Fail, Errors = errors }, JsonRequestBehavior.AllowGet);
            }
        }

        #region Dispose

        private bool _disposed;

        [NonAction]
        protected override void Dispose(bool isDisposing)
        {
            if (!_disposed)
            {
                if (isDisposing)
                {
                    OrderService = null;
                    ProductService = null;
                    CurrencyService = null;
                }
                _disposed = true;
            }
            base.Dispose(isDisposing);
        }

        #endregion
    }
}