using Eagle.Core.Configuration;
using Eagle.Core.Pagination;
using Eagle.Core.Settings;
using Eagle.Resources;
using Eagle.Services.Business;
using Eagle.Services.Dtos.Business;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Service;
using Eagle.Services.SystemManagement;
using Eagle.Services.Validations;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Eagle.WebApp.Controllers
{
    public class ProductionController : BasicController
    {
        private IApplicationService ApplicationService { get; set; }
        private IDocumentService DocumentService { get; set; }
        private ICurrencyService CurrencyService { get; set; }
        private ISupplierService SupplierService { get; set; }
        private IProductService ProductService { get; set; }
        private IBookingService BookingService { get; set; }

        public ProductionController(IApplicationService applicationService, IProductService productService,
            ISupplierService supplierService, ICurrencyService currencyService, IDocumentService documentService,
            IBookingService bookingService)
        {
            ApplicationService = applicationService;
            ProductService = productService;
            SupplierService = supplierService;
            CurrencyService = currencyService;
            DocumentService = documentService;
            BookingService = bookingService;
        }

        #region PRODUCT CATEGORY
        //Product Category Combobox tree
        [HttpGet]
        public ActionResult GetProductCategorySelectTree(int? selectedId, bool? isRootShowed)
        {
            var list = ProductService.GetProductCategorySelectTree(ProductCategoryStatus.Published, selectedId, isRootShowed);
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region PRODUCT
        // GET: Production
        public ActionResult Index()
        {
            return View("../Production/Index");
        }

        public ActionResult CompareProducts(string id)
        {
            string[] listId = id.Remove(id.Length - 1, 1).Split(',');
            List<ProductInfoDetail> listProductInfoDetail = new List<ProductInfoDetail>(); ;
            foreach (var productId in listId)
            {
                var productDetail = ProductService.GetProductDetails(int.Parse(productId));
                listProductInfoDetail.Add(productDetail);
            }
            return View("../Production/CompareProducts", listProductInfoDetail);
        }
       
        public ActionResult LoadSearchForm()
        {
            var minFilteredRangeSetting = ApplicationService.GetProductFilteredPriceRangeSetting(GlobalSettings.DefaultApplicationId, "Min").Setting;
            var maxFilteredRangeSetting = ApplicationService.GetProductFilteredPriceRangeSetting(GlobalSettings.DefaultApplicationId, "Max").Setting;

            ViewBag.ProductFilteredPriceMin = int.Parse(minFilteredRangeSetting.KeyValue);
            ViewBag.ProductFilteredPriceMax = int.Parse(maxFilteredRangeSetting.KeyValue);

            return PartialView("../Production/_SearchForm");
        }

        // GET: /Admin/Production/Search
        //[HttpGet]
        //public ActionResult Search(int? categoryId = null, string productName = null, int? page = 1, int pageSize = 6, int? minPrice = null, int? maxPrice = null)
        //{
        //    int vendorId = ApplicationService.GetCurrentVendorId();
        //    ViewBag.CurrencyCode = ApplicationService.GetCurrentCurrencyCode();
        //    int? recordCount = 0;
        //    var sources = ProductService.GetProducts(vendorId, categoryId, productName, null, null, minPrice, maxPrice, ProductStatus.Published, ref recordCount, "ProductId DESC", page, pageSize);
        //    int currentPageIndex = page - 1 ?? 0;
        //    IPagedList<ProductInfoDetail> lst = sources.ToPagedList(currentPageIndex, pageSize, recordCount);
        //    return PartialView("../Production/_SearchResult", lst);
        //}

        [HttpGet]
        public ActionResult Search(ProductSearchEntry entry, int? page = 1, int pageSize = 9)
        {
            string languageCode = ApplicationService.GetCurrentLanguageCode(GlobalSettings.DefaultApplicationId);
            string currencyCode = ApplicationService.GetCurrentCurrencyCode();
            int vendorId = ApplicationService.GetCurrentVendorId();
            
            entry.Status = ProductStatus.Published;
            ViewBag.CurrencyCode = currencyCode;

            int? recordCount = 0;
            var sources = ProductService.GetProducts(entry, languageCode, vendorId, ref recordCount, "ListOrder DESC", page, pageSize);
            int currentPageIndex = page - 1 ?? 0;
            IPagedList<ProductInfoDetail> lst = sources.ToPagedList(currentPageIndex, pageSize, recordCount);
            return PartialView("../Production/_SearchResult", lst);
        }

        // GET: /Admin/Production/GetProducts
        [HttpGet]
        public ActionResult GetProducts(int? categoryId = null, string searchText = null, int? page = 1, int pageSize = 6)
        {
            string languageCode = ApplicationService.GetCurrentLanguageCode(GlobalSettings.DefaultApplicationId);
            string currencyCode = ApplicationService.GetCurrentCurrencyCode();
            int vendorId = ApplicationService.GetCurrentVendorId();

            ViewBag.CurrencyCode = currencyCode;

            var entry = new ProductSearchEntry
            {
                CategoryId = categoryId,
                ProductName = searchText,
                Status = ProductStatus.Published
            };

            int? recordCount = 0;
            var sources = ProductService.GetProducts(entry, languageCode, vendorId, ref recordCount, "ListOrder DESC", page, pageSize);
            int currentPageIndex = page - 1 ?? 0;
            var lst = sources.ToPagedList(currentPageIndex, pageSize, recordCount);
            return PartialView("../Production/_ProductList", lst);
        }

        // GET: /Admin/Production/ProductsByCategory
        [HttpGet]
        public ActionResult ProductsByCategory([System.Web.Http.FromUri]int categoryId)
        {
            var model = new ProductByCategory
            {
                CategoryId = categoryId,
                Category = new ProductCategoryDetail
                {
                    CategoryId = categoryId
                }
            };

            return View("../Production/ProductsByCategory", model);
        }

        // GET: /Admin/Production/GetProductsByCategory
        [HttpGet]
        public ActionResult GetProductsByCategory(int categoryId, string searchText = null, int? page = 1, int pageSize = 5)
        {
            string languageCode = ApplicationService.GetCurrentLanguageCode(GlobalSettings.DefaultApplicationId);
            string currencyCode = ApplicationService.GetCurrentCurrencyCode();
            int vendorId = ApplicationService.GetCurrentVendorId();

            ViewBag.CurrencyCode = currencyCode;

            var entry = new ProductSearchEntry
            {
                CategoryId = categoryId,
                ProductName = searchText,
                Status = ProductStatus.Published
            };
            
            int? recordCount = 0;
            var sources = ProductService.GetProducts(entry, languageCode, vendorId, ref recordCount, "ProductId DESC", page, pageSize);
            int currentPageIndex = page - 1 ?? 0;
            var lst = sources.ToPagedList(currentPageIndex, pageSize, recordCount);
            var category = ProductService.GetProductCategoryDetails(categoryId);

            var model = new ProductByCategory
            {
                CategoryId = categoryId,
                Category = category,
                Products = lst
            };

            return PartialView("../Production/_ProductsByCategory", model);
        }

        // GET: /Admin/Production/GetProductsByManufacturer
        [HttpGet]
        public ActionResult GetProductsByManufacturer(int manufacturerId, int? page = 1, int pageSize = 5)
        {
            int vendorId = ApplicationService.GetCurrentVendorId();
            int? recordCount = 0;
            var sources = ProductService.GetProductsByManufacturer(vendorId, manufacturerId, ProductStatus.Published, ref recordCount, "ProductId DESC", page, pageSize);
            int currentPageIndex = page - 1 ?? 0;
            var lst = sources.ToPagedList(currentPageIndex, pageSize, recordCount);
            return PartialView("../Production/_ProductsByManufacturer", lst);
        }

        // GET: /Admin/Production/GetDiscountedProduct
        [HttpGet]
        public ActionResult GetDiscountedProducts(int? categoryId = null, int? page = 1, int pageSize = 5)
        {
            int vendorId = ApplicationService.GetCurrentVendorId();
            int? recordCount = 0;
            var sources = ProductService.GetDiscountedProducts(vendorId, categoryId, ProductStatus.Published, ref recordCount, "ProductId DESC", page, pageSize);
            int currentPageIndex = page - 1 ?? 0;
            var lst = sources.ToPagedList(currentPageIndex, pageSize, recordCount);
            return PartialView("../Production/_ProductsByDiscount", lst);
        }

        [HttpGet]
        public ActionResult GetLastestProducts(int? categoryId = null, int? page = 1, int? pageSize = 3)
        {
            int vendorId = ApplicationService.GetCurrentVendorId();
            int? recordCount = 0;
            var sources = ProductService.GetLastestProducts(vendorId, categoryId, ProductStatus.Published, ref recordCount, "ProductId DESC", page, pageSize ?? GlobalSettings.DefaultPageSize);
            int currentPageIndex = page - 1 ?? 0;
            var lst = sources.ToPagedList(currentPageIndex, pageSize ?? GlobalSettings.DefaultPageSize, recordCount);
            return PartialView("../Production/_ProductsByLastest", lst);
        }

        // GET: /Admin/Production/GetRelatedProducts
        [HttpGet]
        public ActionResult GetRelatedProducts(int? categoryId = null, int? page = 1)
        {
            string languageCode = ApplicationService.GetCurrentLanguageCode(GlobalSettings.DefaultApplicationId);
            string currencyCode = ApplicationService.GetCurrentCurrencyCode();
            int vendorId = ApplicationService.GetCurrentVendorId();

            ViewBag.CurrencyCode = currencyCode;

            var entry = new ProductSearchEntry
            {
                CategoryId = categoryId,
                Status = ProductStatus.Published
            };
            
            int? recordCount = 0;
            var sources = ProductService.GetProducts(entry, languageCode, vendorId, ref recordCount, "ProductId DESC", page, GlobalSettings.DefaultPageSizeByLastest);
            int currentPageIndex = page - 1 ?? 0;
            var lst = sources.ToPagedList(currentPageIndex, GlobalSettings.DefaultPageSizeByLastest, recordCount);
            return PartialView("../Production/_RelatedProducts", lst);
        }

        // GET: /Admin/Product/Details/5        
        [HttpGet]
        public ActionResult Details(int id)
        {
            var item = ProductService.GetProductDetails(id);
            if (!string.IsNullOrEmpty(item.Specification))
            {
                item.Specification = HttpUtility.HtmlDecode(item.Specification);
            }
            // Get product album
            var productAlbums = ProductService.GetProductAlbum(item.ProductId);
            List<ProductEditEntry> listProductAlbum = new List<ProductEditEntry>();
            foreach (var productAlbum in productAlbums)
            {
                var fileInfo = DocumentService.GetFileInfoDetail(productAlbum.FileId);
                ProductEditEntry productEditEntry = new ProductEditEntry();
                if (fileInfo != null)
                {
                    productEditEntry.FileUrl = fileInfo.FileUrl;
                    productEditEntry.ProductId = item.ProductId;
                    productEditEntry.LargePhoto = fileInfo.FileId;
                    listProductAlbum.Add(productEditEntry);
                }
            }
            ViewBag.ProductAlbums = listProductAlbum;
            ViewBag.ProductComments = item.Comments.Count(x => x.IsActive == ProductCommentStatus.Active);
            return View("../Production/ProductDetail", item);
        }

        // GET: /Admin/Production/Details/ComparativeProducts
        [HttpGet]
        public ActionResult GetComparativeProducts(int? categoryId = null, decimal? price = 0, int? page = 1)
        {
            int? minPrice = null;
            int? maxPrice = null;
            if (price.HasValue)
            {
                var minRangeSetting = ApplicationService.GetProductPriceRangeSetting(GlobalSettings.DefaultApplicationId, "Min").Setting;
                var maxRangeSetting = ApplicationService.GetProductPriceRangeSetting(GlobalSettings.DefaultApplicationId, "Max").Setting;

                int minPercent = int.Parse(minRangeSetting.KeyValue);
                int maxPercent = int.Parse(maxRangeSetting.KeyValue);

                minPrice = (int)(price.Value - price.Value * minPercent / 100);
                maxPrice = (int)(price.Value + price.Value * maxPercent / 100);

                //string appSetting = ApplicationService.GetDefaultApplicationSetting(GlobalSettings.DefaultApplicationId, "PriceRange");
                //if(!string.IsNullOrEmpty(appSetting))
                //{
                //    int percent = int.Parse(appSetting.Split(',')[1]);
                //    minPrice = (int)(price.Value - price.Value * percent / 100);
                //    maxPrice = (int)(price.Value + price.Value * percent / 100);
                //}
            }

            string languageCode = ApplicationService.GetCurrentLanguageCode(GlobalSettings.DefaultApplicationId);
            string currencyCode = ApplicationService.GetCurrentCurrencyCode();
            int vendorId = ApplicationService.GetCurrentVendorId();

            ViewBag.CurrencyCode = currencyCode;

            var entry = new ProductSearchEntry
            {
                CategoryId = categoryId,
                MinPrice = minPrice,
                MaxPrice = maxPrice,
                Status = ProductStatus.Published
            };

            int ? recordCount = 0;
            var sources = ProductService.GetProducts(entry, languageCode, vendorId, ref recordCount, "ProductId DESC", page, GlobalSettings.DefaultPageSize);
            int currentPageIndex = page - 1 ?? 0;
            var lst = sources.ToPagedList(currentPageIndex, GlobalSettings.DefaultPageSize, recordCount);
            return PartialView("../Production/_ComparativeProducts", lst);
        }

        [HttpPut]
        public ActionResult UpdateTotalViews(int id)
        {
            try
            {
                ProductService.UpdateProductTotalViews(id);
                return Json(new SuccessResult { Status = ResultStatusType.Success, Data = LanguageResource.UpdateNewTotalViewSuccess }, JsonRequestBehavior.AllowGet);
            }
            catch (ValidationError ex)
            {
                var errors = ValidationExtension.GetException(ex);
                return Json(new FailResult { Status = ResultStatusType.Fail, Errors = errors }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region PRODUCT COMMENTS

        // GET: /Admin/Production/GetComments
        [HttpGet]
        public ActionResult GetComments(int productId)
        {
            var sources = ProductService.GeProductComments(productId, ProductCommentStatus.Active);
            return PartialView("../Production/_ProductComments", sources);
        }

        // GET: /Admin/Production/PostComment
        [HttpGet]
        public ActionResult CreateComment(int productId)
        {
            var item = ApplicationService.GetRecaptchaSetting(GlobalSettings.DefaultApplicationId, GoogleReCaptcha.SiteKey);
            ViewBag.DataSiteKey = item.Setting.KeyValue;

            var entry = new ProductCommentEntry
            {
                ProductId = productId
            };
            return PartialView("../Production/_ProductCommentPost", entry);
        }

        [HttpPost]
        public ActionResult CreateComment(int id, string newParam)
        {
            //Validate Google recaptcha here
            var response = Request["g-recaptcha-response"];
            var item = ApplicationService.GetRecaptchaSetting(GlobalSettings.DefaultApplicationId, GoogleReCaptcha.SecretKey);
            string secretKey = item.Setting.KeyValue;
            var client = new WebClient();
            var result = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secretKey, response));
            var obj = JObject.Parse(result);
            var status = (bool)obj.SelectToken("success");
            TempData["Captcha"] = status;
            return RedirectToAction("Details", new { id = id });
        }

        // POST: /Admin/Production/CreateProductComment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateProductComment(ProductCommentEntry entry)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    ProductService.InsertProductComment(entry);
                    return Json(new SuccessResult { Status = ResultStatusType.Success, Data = LanguageResource.CreateSuccess }, JsonRequestBehavior.AllowGet);
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
        #endregion

        #region RATINGS

        [HttpPost]
        public ActionResult CreateProductRating(int? customerId, int id, int rate)
        {
            try
            {
                var rateEntry = new ProductRatingEntry
                {
                    CustomerId = customerId,
                    ProductId = id,
                    Rate = rate,
                    TotalRates = ProductService.GetDefaultProductRating(GlobalSettings.DefaultApplicationId)
                };
                var averageRates = ProductService.InsertProductRating(rateEntry);
                return Json(new SuccessResult
                {
                    Status = ResultStatusType.Success,
                    Data = new
                    {
                        AverageRates = averageRates,
                        Message = LanguageResource.CreateSuccess
                    }
                }, JsonRequestBehavior.AllowGet);
            }
            catch (ValidationError ex)
            {
                var errors = ValidationExtension.GetException(ex);
                return Json(new FailResult { Status = ResultStatusType.Fail, Errors = errors }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion


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
                    DocumentService = null;
                    CurrencyService = null;
                    SupplierService = null;
                    ProductService = null;
                }
                _disposed = true;
            }
            base.Dispose(isDisposing);
        }

        #endregion
    }
}