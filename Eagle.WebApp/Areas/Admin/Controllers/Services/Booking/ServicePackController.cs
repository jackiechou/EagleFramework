using Eagle.Core.Configuration;
using Eagle.Core.Pagination;
using Eagle.Core.Settings;
using Eagle.Resources;
using Eagle.Services.Business;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Dtos.Services.Booking;
using Eagle.Services.Service;
using Eagle.Services.Validations;
using Eagle.WebApp.Areas.Admin.Controllers.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Eagle.Services;

namespace Eagle.WebApp.Areas.Admin.Controllers.Services.Booking
{
    public class ServicePackController : BaseController
    {
        private IBookingService BookingService { get; set; }
        private ICurrencyService CurrencyService { get; set; }
        private IEmployeeService EmployeeService { get; set; }

        public ServicePackController(IBookingService bookingService, ICurrencyService currencyService, IEmployeeService employeeService)
            : base(new IBaseService[] { bookingService, currencyService, employeeService })
        {
            BookingService = bookingService;
            CurrencyService = currencyService;
            EmployeeService = employeeService;
        }

        #region GET METHODS =========================================================================

        // GET: Admin/ServicePack
        [HttpGet]
        public ActionResult Index()
        {
            return View("../Services/Booking/ServicePack/Index");
        }

        // GET: /Admin/ServicePack/PoplulateServicePackDurationSelectList       
        [HttpGet]
        public ActionResult PoplulateServicePackDurationSelectList(bool? status = null, int? selectedValue = null, bool? isShowSelectText = true)
        {
            var employee = BookingService.PopulateServicePackDurationSelectList(status, selectedValue, isShowSelectText);
            return Json(employee, JsonRequestBehavior.AllowGet);
        }

        // GET: Hierachical List 
        [HttpGet]
        public ActionResult GetServiceCategorySelectTree(ServiceType typeId = ServiceType.Single, int? selectedId = null, bool? isRootShowed = true)
        {
            var list = BookingService.GetServiceCategorySelectTree(typeId, null, selectedId, isRootShowed);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        // GET: /Admin/ServicePack/Create
        [HttpGet]
        public ActionResult Create()
        {
            var model = new ServicePackEntry
            {
                CurrencyCode = CurrencyService.GetSelectedCurrency().CurrencyCode,
                TypeId = Convert.ToInt32(ServiceType.Single)
            };

            ViewBag.TypeId = BookingService.PopulateServicePackTypeSelectList(ServicePackTypeStatus.Active, null, true);
            ViewBag.AvailableProviders = BookingService.PopulateAvailableProviderMultiSelectList(null, EmployeeStatus.Published);
            ViewBag.SelectedProviders = BookingService.PopulateSelectedProviderMultiSelectList(null, EmployeeStatus.Published);
            ViewBag.DiscountId = BookingService.PopulateServiceDiscountSelectList(ServiceDiscountStatus.Active, null, false);
            ViewBag.TaxRateId = BookingService.PopulateServiceTaxRateSelectList(true);

            return View("../Services/Booking/ServicePack/Create", model);
        }

        //GET: /Admin/ServicePack/Edit/5        
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var entity = BookingService.GetServicePackDetail(id);
            var editModel = new ServicePackEditEntry
            {
                CategoryId = entity.CategoryId,
                TypeId = entity.TypeId,
                Capacity = entity.Capacity,
                AvailableQuantity = entity.AvailableQuantity,
                DurationId = entity.DurationId,
                DiscountId = entity.DiscountId,
                TaxRateId = entity.TaxRateId,
                PackageId = entity.PackageId,
                PackageCode = entity.PackageCode,
                PackageName = entity.PackageName,
                Weight = entity.Weight,
                PackageFee = entity.PackageFee,
                CurrencyCode = entity.CurrencyCode,
                TotalFee = entity.TotalFee,
                Description = entity.Description,
                Specification = entity.Specification,
                Status = entity.Status,
                FileId = entity.FileId
            };

            if (entity.Document != null)
            {
                editModel.FileUrl = entity.Document.FileUrl;
            }

            // load options
            var options = entity.Options.ToList();
            if (options.Any())
            {
                var optionsLst = new List<ServicePackOptionEditEntry>();
                optionsLst.AddRange(options.Select(option => new ServicePackOptionEditEntry
                {
                    PackageId = editModel.PackageId,
                    OptionId = option.OptionId,
                    OptionName = option.OptionName,
                    OptionValue = option.OptionValue,
                    IsActive = option.IsActive,
                }));
                editModel.ExistedOptions = optionsLst;
            }

            ViewBag.TypeId = BookingService.PopulateServicePackTypeSelectList(ServicePackTypeStatus.Active, entity.TypeId, true);
            ViewBag.AvailableProviders = BookingService.PopulateAvailableProviderMultiSelectList(entity.PackageId, EmployeeStatus.Published);
            ViewBag.SelectedProviders = BookingService.PopulateSelectedProviderMultiSelectList(entity.PackageId, EmployeeStatus.Published);
            ViewBag.DiscountId = BookingService.PopulateServiceDiscountSelectList(ServiceDiscountStatus.Active, entity.DiscountId, false);
            ViewBag.TaxRateId = BookingService.PopulateServiceTaxRateSelectList(true, entity.TaxRateId);

            return View("../Services/Booking/ServicePack/Edit", editModel);
        }

        // GET: /Admin/ServicePack/Search       
        [HttpGet]
        public ActionResult LoadSearchForm()
        {
            ViewBag.ServiceType = BookingService.PopulateServicePackTypeSelectList(ServicePackTypeStatus.Active, null, true);
            ViewBag.IsActive = BookingService.PopulateServicePackStatus();
            return PartialView("../Services/Booking/ServicePack/_SearchForm");
        }

        //GET: /Admin/ServicePack/Search
        [HttpGet]
        public ActionResult Search(ServicePackSearchEntry filter, string sourceEvent, int? page)
        {
            if (String.IsNullOrEmpty(sourceEvent))
            {
                TempData["ServicePackSearchRequest"] = filter;
            }
            else
            {
                if (TempData["ServicePackSearchRequest"] != null)
                {
                    filter = (ServicePackSearchEntry)TempData["ServicePackSearchRequest"];
                }
            }
            TempData.Keep();

            int? recordCount = 0;
            var sources = BookingService.GetServicePacks(filter, ref recordCount, "ListOrder DESC", page, GlobalSettings.DefaultPageSize);
            int currentPageIndex = page - 1 ?? 0;
            var lst = sources.ToPagedList(currentPageIndex, GlobalSettings.DefaultPageSize, recordCount);
            return PartialView("../Services/Booking/ServicePack/_SearchResult", lst);
        }

        #endregion =====================================================================================

        #region INSERT - UPDATE - DELETE METHODS =======================================================

        // POST: /Admin/ServicePack/Create
        [HttpPost]
        public ActionResult Create(ServicePackEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    BookingService.InsertServicePack(ApplicationId, UserId, entry);
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


        // PUT - /Admin/ServicePack/Edit
        [HttpPut]
        public ActionResult Edit(ServicePackEditEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    BookingService.UpdateServicePack(ApplicationId, UserId, entry);
                    return Json(new SuccessResult
                    {
                        Status = ResultStatusType.Success,
                        Data = new
                        {
                            Message = LanguageResource.UpdateSuccess,
                            Id = entry.PackageId
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


        // POST: /Admin/ServicePack/UpdateStatus/5
        [HttpPut]
        public ActionResult UpdateStatus(int id, bool status)
        {
            try
            {
                BookingService.UpdateServicePackStatus(id, status ? ServicePackStatus.Active : ServicePackStatus.InActive);
                return Json(new SuccessResult { Status = ResultStatusType.Success, Data = LanguageResource.UpdateStatusSuccess }, JsonRequestBehavior.AllowGet);
            }
            catch (ValidationError ex)
            {
                var errors = ValidationExtension.GetException(ex);
                return Json(new FailResult { Status = ResultStatusType.Fail, Errors = errors }, JsonRequestBehavior.AllowGet);
            }
        }


        //DELETE: /Admin/ServicePack/Delete/5
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            try
            {
                BookingService.DeleteServicePack(id);
                return Json(new SuccessResult { Status = ResultStatusType.Success, Data = LanguageResource.DeleteSuccess }, JsonRequestBehavior.AllowGet);
            }
            catch (ValidationError ex)
            {
                var errors = ValidationExtension.GetException(ex);
                return Json(new FailResult { Status = ResultStatusType.Fail, Errors = errors }, JsonRequestBehavior.AllowGet);
            }
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
                    EmployeeService = null;
                }
                _disposed = true;
            }
            base.Dispose(isDisposing);
        }

        #endregion
    }
}