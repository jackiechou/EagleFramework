using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Eagle.Core.Configuration;
using Eagle.Core.Pagination;
using Eagle.Core.Settings;
using Eagle.Resources;
using Eagle.Services.Business;
using Eagle.Services.Dtos.Business;
using Eagle.Services.Dtos.Common;
using Eagle.Services.SystemManagement;
using Eagle.Services.Validations;
using Eagle.WebApp.Areas.Admin.Controllers.Common;
using System;
using Eagle.Services;

namespace Eagle.WebApp.Areas.Admin.Controllers.Business.Ecommerce
{
    public class ProductController : BaseController
    {
        private ICurrencyService CurrencyService { get; set; }
        private IDocumentService DocumentService { get; set; }
        private ISupplierService SupplierService { get; set; }
        private IProductService ProductService { get; set; }

        public ProductController(IProductService productService, ISupplierService supplierService, ICurrencyService currencyService, IDocumentService documentService)
            : base(new IBaseService[] { productService, supplierService, currencyService, documentService })
        {
            ProductService = productService;
            SupplierService = supplierService;
            CurrencyService = currencyService;
            DocumentService = documentService;
        }

        #region GET METHODS =========================================================================
        //
        // GET: /Admin/Product/
        public ActionResult Index()
        {
            return View("../Business/Ecommerce/Product/Index");
        }

        //
        // GET: /Admin/Product/CreateProductAttribute
        [HttpGet]
        public ActionResult CreateProductAttribute()
        {
            return PartialView("../Business/Ecommerce/Product/_CreateProductAttribute");
        }

        // GET: /Admin/ProductAttributeOption/Create        
        [HttpGet]
        public ActionResult CreateProductAttributeOption(string mode, int index)
        {
            if (mode == "create")
            {
                var option = new ProductAttributeOptionEntry
                {
                    AttributeIndex = index
                };
                return PartialView("../Business/Ecommerce/Product/_CreateProductAttributeOption", option);
            }
            else
            {
                var attribute = new ProductAttributeEditEntry
                {
                    Index = index
                };
                return PartialView("../Business/Ecommerce/Product/_EditProductAttributeOption", attribute);
            }
        }
      
        //
        // GET: /Admin/Product/GetProductAttributeOptions
        [HttpGet]
        public ActionResult GetProductAttributeOptions(int attributeId, int? index=0)
        {
            var attribute = new ProductAttributeEditEntry();
            var item = ProductService.GetProductAttributeDetails(attributeId);
            if (item != null)
            {
                var optionsLst = new List<ProductAttributeOptionEditEntry>();
                var options = ProductService.GeAttributeOptions(attributeId).ToList();
                if (options.Any())
                {
                    optionsLst.AddRange(options.Select(option => new ProductAttributeOptionEditEntry
                    {
                        AttributeIndex = index,
                        AttributeId = attributeId,
                        OptionId = option.OptionId,
                        OptionName = option.OptionName,
                        OptionValue = option.OptionValue,
                        IsActive = option.IsActive,
                    }));
                }

                attribute.Index = index;
                attribute.AttributeId = attribute.AttributeId;
                attribute.AttributeName = attribute.AttributeName;
                attribute.IsActive = attribute.IsActive;
                attribute.ExistedOptions = optionsLst;
            }
            return PartialView("../Business/Ecommerce/Product/_EditProductAttributeOption", attribute);
        }


        //
        // GET: /Admin/Product/GetProductAttributes
        [HttpGet]
        public ActionResult EditProductAttribute(int productId)
        {
            int index = 0;
            var model = new ProductEditEntry();
            var attributes = ProductService.GeAttributes(productId).ToList();
            if (attributes.Any())
            {
                var attributeLst = new List<ProductAttributeEditEntry>();
                foreach (var attribute in attributes)
                {
                    var optionsLst = new List<ProductAttributeOptionEditEntry>();
                    var options = ProductService.GeAttributeOptions(attribute.AttributeId).ToList();
                    if (options.Any())
                    {
                        optionsLst.AddRange(options.Select(option => new ProductAttributeOptionEditEntry
                        {
                            AttributeIndex = index++,
                            AttributeId = attribute.AttributeId,
                            OptionId = option.OptionId,
                            OptionName = option.OptionName,
                            OptionValue = option.OptionValue,
                            IsActive = option.IsActive,
                        }));
                    }

                    attributeLst.Add(new ProductAttributeEditEntry
                    {
                        Index = index++,
                        AttributeId = attribute.AttributeId,
                        AttributeName = attribute.AttributeName,
                        IsActive = attribute.IsActive,
                        ExistedOptions = optionsLst
                    });
                }

                model.ExistedAttributes = attributeLst;
            }
            return PartialView("../Business/Ecommerce/Product/_EditProductAttribute", model);
        }

        // GET: /Admin/Product/Create        
        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.CurrencyCode = CurrencyService.GetSelectedCurrency().CurrencyCode;
            ViewBag.ManufacturerId = SupplierService.PoplulateManufacturerCategorySelectList(ManufacturerCategoryStatus.Active);
            ViewBag.DiscountId = ProductService.PopulateProductDiscountSelectList(DiscountType.Normal, ProductDiscountStatus.Active);
            ViewBag.TaxRateId = ProductService.PopulateProductTaxRateSelectList(ProductTaxRateStatus.Active);
            ViewBag.ProductCode = ProductService.GenerateProductCode(15);
            ViewBag.BrandId = ProductService.PopulateProductBrandSelectList(BrandStatus.Active);

            return View("../Business/Ecommerce/Product/Create");
        }

        //Product Category Combobox tree
        [HttpGet]
        public ActionResult GetProductCategorySelectTree(int? selectedId, bool? isRootShowed)
        {
            var list = ProductService.GetProductCategorySelectTree(ProductCategoryStatus.Published, selectedId, isRootShowed);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        //Product Type by Product Category Id
        [HttpGet]
        public ActionResult GetProductTypeSelectList(int categoryId, int? selectedValue, bool? isShowSelectText)
        {
            var list = ProductService.PoplulateProductTypeSelectList(categoryId, ProductTypeStatus.Active, selectedValue, isShowSelectText);
            return Json(list, JsonRequestBehavior.AllowGet);
        }


        // GET: /Admin/Product/Edit/5        
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var item = ProductService.GetProductDetail(id);

            var model = new ProductEditEntry
            {
                ProductId = item.ProductId,
                ProductCode = item.ProductCode,
                CategoryId = item.CategoryId,
                ManufacturerId = item.ManufacturerId,
                ProductTypeId = item.ProductTypeId,
                ProductName = item.ProductName,
                DiscountId = item.DiscountId,
                VendorId = item.VendorId,
                CurrencyCode = item.CurrencyCode,
                NetPrice = item.NetPrice,
                GrossPrice = item.GrossPrice,
                TaxRateId = item.TaxRateId,
                UnitsInStock = item.UnitsInStock,
                UnitsOnOrder = item.UnitsOnOrder,
                UnitsInAPackage = item.UnitsInAPackage,
                UnitsInBox = item.UnitsInBox,
                Unit = item.Unit,
                Weight = item.Weight,
                UnitOfWeightMeasure = item.UnitOfWeightMeasure,
                Length = item.Length,
                Width = item.Width,
                Height = item.Height,
                UnitOfDimensionMeasure = item.UnitOfDimensionMeasure,
                Url = item.Url,
                MinPurchaseQty = item.MinPurchaseQty,
                MaxPurchaseQty = item.MaxPurchaseQty,
                Views = item.Views,
                SmallPhoto = item.SmallPhoto,
                LargePhoto = item.LargePhoto,
                ShortDescription = item.ShortDescription,
                Specification = HttpUtility.HtmlDecode(item.Specification),
                Availability = item.Availability,
                StartDate = item.StartDate,
                EndDate = item.EndDate,
                PurchaseScope = item.PurchaseScope,
                Warranty = item.Warranty,
                IsOnline = item.IsOnline,
                InfoNotification = item.InfoNotification,
                PriceNotification = item.PriceNotification,
                QtyNotification = item.QtyNotification,
                Status = item.Status,
                BrandId = item.BrandId,
            };

            if (item.SmallPhoto != null)
            {
                var fileInfo = DocumentService.GetFileInfoDetail((int)item.SmallPhoto);
                if (fileInfo != null)
                {
                    model.FileUrl = fileInfo.FileUrl;
                }
            }

            ViewBag.CurrencyCode = CurrencyService.GetSelectedCurrency().CurrencyCode;
            ViewBag.ManufacturerId = SupplierService.PoplulateManufacturerCategorySelectList(ManufacturerCategoryStatus.Active, item.ManufacturerId);
            ViewBag.DiscountId = ProductService.PopulateProductDiscountSelectList(DiscountType.Normal, ProductDiscountStatus.Active, item.DiscountId);
            ViewBag.TaxRateId = ProductService.PopulateProductTaxRateSelectList(ProductTaxRateStatus.Active, item.TaxRateId);
            ViewBag.BrandId = ProductService.PopulateProductBrandSelectList(BrandStatus.Active);

            //Get product album files
            List<ProductAlbumEditEntry> listOfProductAlbum = new List<ProductAlbumEditEntry>();
            var productAlbums = ProductService.GetProductAlbum(item.ProductId).ToList();
            if (productAlbums.Any())
            {
                foreach (var productAlbum in productAlbums)
                {
                    var fileInfo = DocumentService.GetFileInfoDetail(productAlbum.FileId);
                    if (fileInfo != null)
                    {
                        //@Html.Raw(Json.Encode(arProductAlbums))
                        listOfProductAlbum.Add(new ProductAlbumEditEntry
                        {
                            ProductId = item.ProductId,
                            FileId = fileInfo.FileId,
                            FileUrl = fileInfo.FileUrl
                        });
                    }
                }
            }
            model.ExistedProductAlbumFiles = listOfProductAlbum;
        
            return View("../Business/Ecommerce/Product/Edit", model);
        }

        // GET: /Admin/Product/Search
        [HttpGet]
        public ActionResult LoadSearchForm()
        {
            return PartialView("../Business/Ecommerce/Product/_SearchForm");
        }

        // GET: /Admin/Product/Search
        [HttpGet]
        public ActionResult Search(ProductSearchEntry filter, string sourceEvent, int? page = 1)
        {
            if (String.IsNullOrEmpty(sourceEvent))
            {
                TempData["ProductSearchRequest"] = filter;
            }
            else
            {
                if (TempData["ProductSearchRequest"] != null)
                {
                    filter = (ProductSearchEntry)TempData["ProductSearchRequest"];
                }
            }
            TempData.Keep();

            int? recordCount = 0;
            var sources = ProductService.GetProductList(VendorId, LanguageCode, filter, ref recordCount, "ProductId DESC", page, GlobalSettings.DefaultPageSize);
            int currentPageIndex = page - 1 ?? 0;
            var lst = sources.ToPagedList(currentPageIndex, GlobalSettings.DefaultPageSize, recordCount);
            return PartialView("../Business/Ecommerce/Product/_SearchResult", lst);
        }

        #endregion =====================================================================================

        #region INSERT - UPDATE - DELETE METHODS =======================================================

        // POST: /Admin/Product/Create
        [HttpPost]
        public ActionResult Create(ProductEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ProductService.InsertProduct(ApplicationId, UserId, VendorId, LanguageCode, entry);
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

        //
        // PUT - /Admin/Product/Edit
        [HttpPut]
        public ActionResult Edit(ProductEditEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ProductService.UpdateProduct(ApplicationId, UserId, VendorId, LanguageCode, entry);
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

        //
        // POST: /Admin/Product/UpdateStatus/5
        [HttpPut]
        public ActionResult UpdateStatus(int id, int status)
        {
            try
            {
                ProductService.UpdateProductStatus(id, (ProductStatus)status);
                return Json(new SuccessResult { Status = ResultStatusType.Success, Data = LanguageResource.UpdateStatusSuccess }, JsonRequestBehavior.AllowGet);
            }
            catch (ValidationError ex)
            {
                var errors = ValidationExtension.GetException(ex);
                return Json(new FailResult { Status = ResultStatusType.Fail, Errors = errors }, JsonRequestBehavior.AllowGet);
            }
        }
        
        //
        // DELETE: /Admin/Product/Delete/5
        [HttpDelete]
        public ActionResult Delete([System.Web.Http.FromBody]int id)
        {
            try
            {
                ProductService.DeleteProduct(id);
                return Json(new SuccessResult { Status = ResultStatusType.Success, Data = LanguageResource.DeleteSuccess }, JsonRequestBehavior.AllowGet);
            }
            catch (ValidationError ex)
            {
                var errors = ValidationExtension.GetException(ex);
                return Json(new FailResult { Status = ResultStatusType.Fail, Errors = errors }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult DeleteImage(string key)
        {
            try
            {
                if (!string.IsNullOrEmpty(key))
                {
                    var keys = key.Split('_');
                    var productId = int.Parse(keys[0]);
                    var fileId = int.Parse(keys[1]);
                    ProductService.DeleteImageProductAlbum(productId, fileId);
                    DocumentService.DeleteFile(fileId);
                    return Json(new SuccessResult { Status = ResultStatusType.Success, Data = LanguageResource.DeleteSuccess }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    throw new ArgumentNullException();
                }
              
            }
            catch (ValidationError ex)
            {
                var errors = ValidationExtension.GetException(ex);
                return Json(new FailResult { Status = ResultStatusType.Fail, Errors = errors }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion ==============================================================================

        #region Dispose

        private bool _disposed;

        [NonAction]
        protected override void Dispose(bool isDisposing)
        {
            if (!_disposed)
            {
                if (isDisposing)
                {
                    CurrencyService = null;
                    DocumentService = null;
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