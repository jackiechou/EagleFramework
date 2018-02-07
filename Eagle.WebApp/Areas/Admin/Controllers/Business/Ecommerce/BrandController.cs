using System;
using System.Web.Mvc;
using Eagle.Common.Extensions;
using Eagle.Core.Configuration;
using Eagle.Core.Pagination;
using Eagle.Core.Settings;
using Eagle.Resources;
using Eagle.Services;
using Eagle.Services.Business;
using Eagle.Services.Dtos.Business;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Validations;
using Eagle.WebApp.Areas.Admin.Controllers.Common;

namespace Eagle.WebApp.Areas.Admin.Controllers.Business.Ecommerce
{
    public class BrandController : BaseController
    {
        private IBrandService BrandService { get; set; }
        public BrandController(IBrandService brandService) : base(new IBaseService[] { brandService })
        {
            BrandService = brandService;
        }

        #region GET METHODS

        // GET: Admin/Brand
        public ActionResult Index()
        {
            return View("../Business/Ecommerce/Brand/Index");
        }

        // GET: /Admin/Brand/Create
        [HttpGet]
        public ActionResult Create()
        {
            return PartialView("../Business/Ecommerce/Brand/_Create");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var brand = BrandService.GetBrandDetail(id);

            var brandEntry = new BrandEditEntry()
            {
                BrandId = brand.BrandId,
                BrandName = brand.BrandName,
                BrandAlias = brand.BrandAlias,
                IsOnline = brand.IsOnline? BrandStatus.Active: BrandStatus.InActive,
            };

            return PartialView("../Business/Ecommerce/Brand/_Edit", brandEntry);
        }

        [HttpGet]
        public ActionResult LoadSearchForm()
        {
            return PartialView("../Business/Ecommerce/Brand/_SearchForm");
        }

        [HttpGet]
        public ActionResult Search(BrandSearchEntry searchEntry, string sourceEvent, int? page = 1)
        {
            if (sourceEvent.IsNullOrEmpty())
            {
                TempData["BrandSearchRequest"] = searchEntry;
            }
            else
            {
                if (TempData["BrandSearchRequest"] != null)
                {
                    searchEntry = (BrandSearchEntry)TempData["BrandSearchRequest"];
                }
            }
            TempData.Keep();
            int? recordCount = 0;
            var list = BrandService.GetBrandList(searchEntry, ref recordCount, "BrandId DESC", page, GlobalSettings.DefaultPageSize);
            var currentPageIndex = page - 1 ?? 0;
            var lst = list.ToPagedList(currentPageIndex, GlobalSettings.DefaultPageSize, recordCount);
            return PartialView("../Business/Ecommerce/Brand/_SearchResult", lst);
        }

        #endregion GET METHODS

        #region INSERT - UPDATE - DELETE METHODS

        public ActionResult Create(BrandEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    BrandService.CreateBrand(entry);
                    return Json(new SuccessResult { Status = ResultStatusType.Success, Data = LanguageResource.CreateSuccess }, JsonRequestBehavior.AllowGet);
                }

                var modeStateErrors = ModelState.GetModelStateErrors();
                return Json(new FailResult { Status = ResultStatusType.Fail, Errors = modeStateErrors }, JsonRequestBehavior.AllowGet);
            }
            catch (ValidationError ex)
            {
                var errors = ValidationExtension.GetException(ex);
                return Json(new FailResult { Status = ResultStatusType.Fail, Errors = errors }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPut]
        public ActionResult Edit(BrandEditEntry brandEdit)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    BrandService.UpdateBrand(brandEdit);
                    return Json(new SuccessResult
                    {
                        Status = ResultStatusType.Success,
                        Data = new
                        {
                            Message = LanguageResource.UpdateSuccess,
                            Id = brandEdit.BrandId
                        }
                    }, JsonRequestBehavior.AllowGet);
                }

                var modeStateErrors = ModelState.GetModelStateErrors();
                return Json(new FailResult { Status = ResultStatusType.Fail, Errors = modeStateErrors }, JsonRequestBehavior.AllowGet);
            }
            catch (ValidationError ex)
            {
                var errors = ValidationExtension.GetException(ex);
                return Json(new FailResult { Status = ResultStatusType.Fail, Errors = errors }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPut]
        public ActionResult UpdateStatus(int id, bool status)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    BrandService.UpdateStatus(id, status ? BrandStatus.Active : BrandStatus.InActive);
                    return Json(new SuccessResult
                    {
                        Status = ResultStatusType.Success,
                        Data = new
                        {
                            Message = LanguageResource.UpdateSuccess,
                            Id = id
                        }
                    }, JsonRequestBehavior.AllowGet);
                }

                var modeStateErrors = ModelState.GetModelStateErrors();
                return Json(new FailResult { Status = ResultStatusType.Fail, Errors = modeStateErrors }, JsonRequestBehavior.AllowGet);
            }
            catch (ValidationError ex)
            {
                var errors = ValidationExtension.GetException(ex);
                return Json(new FailResult { Status = ResultStatusType.Fail, Errors = errors }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpDelete]
        public ActionResult Delete([System.Web.Http.FromBody] int id)
        {
            try
            {
                BrandService.DeleteBrand(id);
                return Json(new SuccessResult { Status = ResultStatusType.Success, Data = LanguageResource.DeleteSuccess }, JsonRequestBehavior.AllowGet);
            }
            catch (ValidationError ex)
            {
                var errors = ValidationExtension.GetException(ex);
                return Json(new FailResult { Status = ResultStatusType.Fail, Errors = errors }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion INSERT - UPDATE - DELETE METHODS

        #region Dispose

        private bool _disposed;

        [NonAction]
        protected override void Dispose(bool isDisposing)
        {
            if (!_disposed)
            {
                if (isDisposing)
                {
                    BrandService = null;
                }
                _disposed = true;
            }
            base.Dispose(isDisposing);
        }

        #endregion
    }
}