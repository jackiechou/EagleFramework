using System;
using Eagle.Services.Business;
using System.Web.Mvc;
using Eagle.Core.Configuration;
using Eagle.Core.Settings;
using Eagle.Services.Dtos.Business.Transaction;
using Eagle.Services.Service;
using Eagle.Services.SystemManagement;

namespace Eagle.WebApp.Controllers
{
    public class CartController : Controller
    {
        private IApplicationService ApplicationService { get; set; }
        private IProductService ProductService { get; set; }
        private IBookingService BookingService { get; set; }
        private ICartService CartService { get; set; }

        public CartController(IApplicationService applicationService, ICartService cartService, IBookingService bookingService, IProductService productService)
        {
            CartService = cartService;
            ApplicationService = applicationService;
            BookingService = bookingService;
            ProductService = productService;
        }

        public ActionResult Index()
        {
            var cart = ShoppingCart.Instance;
            CartService.ValidateCart(ShoppingCart.Instance);

            var dueDateRangeOfOrder = ApplicationService.GetOrderSetting(GlobalSettings.DefaultApplicationId, OrderSetting.DueDateRange).Setting.KeyValue;
            DateTime dueDate = DateTime.UtcNow.AddDays(Convert.ToInt32(dueDateRangeOfOrder));

            var info = new CartInfoForShopping
            {
                OrderDate = DateTime.UtcNow,
                DueDate = dueDate,
                Count = cart.Count,
                Weights = cart.Weights,
                SubTotal = cart.SubTotal,
                Total = cart.Total,
                Tax = cart.Tax,
                Discount = cart.Discount,
                CurrencyCode = cart.CurrencyCode,
                Items = cart.Items
            };
            return View("../Cart/Index", info);
        }

        [HttpGet]
        public ActionResult LoadCartInfo()
        {
            var cart = ShoppingCart.Instance;
            CartService.ValidateCart(ShoppingCart.Instance);
            return PartialView("../Cart/_CartInfo", cart);
        }

        [HttpPost]
        public ActionResult Add(int id, int quantity, ItemType type, int? periodGroupId=null, int? fromPeriod = null, int? toPeriod = null, string comment=null)
        {
            var applicationId = GlobalSettings.DefaultApplicationId;
            var cart = ShoppingCart.Instance;
            cart.Add(applicationId, id, quantity, type);
            CartService.ValidateCart(cart);
            return Json(cart, JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult Remove(int id)
        {
            var cart = ShoppingCart.Instance;
            cart.Remove(id);
            CartService.ValidateCart(cart);
            return Json(cart, JsonRequestBehavior.AllowGet);
        }
        
        [HttpPost]
        public ActionResult Update(int id, int quantity)
        {
            var cart = ShoppingCart.Instance;
            cart.Update(id, quantity);
            CartService.ValidateCart(cart);
            return Json(cart, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Clear()
        {
            var cart = ShoppingCart.Instance;
            cart.Clear();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult GetItemDetail(int id)
        {
            var itemProduct = ProductService.GetProductDetails(id);
            if (itemProduct != null)
            {
                return PartialView("../Cart/_ProductDetailDialog", itemProduct);
            }
            else
            {
                var itemService = BookingService.GetServicePackDetail(id);
                return PartialView("../Cart/_ServicePackDetailDialog", itemService);
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
                    ApplicationService = null;
                    BookingService = null;
                    ProductService = null;
                    CartService = null;
                }
                _disposed = true;
            }
            base.Dispose(isDisposing);
        }

        #endregion
    }
}